using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Application.Services;
using Domain.Entity;
using Domain.Exception;
using HtmlAgilityPack;
using Infrastructure.Services.FetchDtos;
using Infrastructure.Util;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public partial class DataFetchService : IDataFetchService
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly HtmlWeb _htmlLoader = new();
    private readonly HttpClient _httpClient = new();
    private readonly ILogger<DataFetchService> _logger;

    public DataFetchService(ILogger<DataFetchService> logger, IApplicationDbContextFactory applicationDbContextFactory)
    {
        _logger = logger;
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    public async Task FetchCurrentLeague()
    {
        var doc = _htmlLoader.Load(PoeToolUrls.PoeDbLeagueUrl);
        if (doc is null) throw new PoeDbDownException();

        var tables = doc.DocumentNode.SelectNodes("//table");
        if (tables is null) throw new PoeDbCannotParseException("No tables found");
        var leaguesTable = tables.FirstOrDefault(n => n.HasChildNodes && n.InnerHtml.Contains("Weeks"));
        if (leaguesTable is null) throw new PoeDbCannotParseException("No league table found");

        var leagueRows = leaguesTable.SelectNodes(".//tr").Where(n => n.HasChildNodes).ToArray();
        if (leagueRows.Length == 0) throw new PoeDbCannotParseException("No league rows found");

        var titleRow = leagueRows[0];
        var (versionColumn, nameColumn, releaseColumn) = GetIndexesFromTitleRow(titleRow);

        await ParseLeagueRows(
            leagueRows.Skip(1).Where(row => row.HasChildNodes),
            releaseColumn,
            nameColumn,
            versionColumn
        );
    }

    public async Task FetchCurrencyData(League league)
    {
        var response = await _httpClient.GetAsync($"{PoeToolUrls.PoeNinjaCurrencyUrl}&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new PoeNinjaDownException();
        var currencyPriceData = await response.Content.ReadFromJsonAsync<CurrencyPriceData>();
        if (currencyPriceData is null) throw new PoeNinjaDownException();
        _logger.LogInformation("Got data from {Result} currency items", currencyPriceData.Lines.Length);

        // set icons
        foreach (var currencyData in currencyPriceData.Lines)
        {
            var currencyDetails =
                currencyPriceData.CurrencyDetails
                                 .FirstOrDefault(currencyDetails =>
                                                     currencyData.Name.EqualsIgnoreCase(currencyDetails.Name));
            if (currencyDetails is not null) currencyData.Icon = currencyDetails.Icon;
        }

        await using var applicationDbContext = (ApplicationDbContext)_applicationDbContextFactory.CreateDbContext();
        var existingCurrency = applicationDbContext.Currency.Select(currency => currency.Id).ToArray();
        applicationDbContext.ClearTrackedEntities();

        var newPoeNinjaCurrencyData = currencyPriceData.Lines
                                                       .Where(currencyData =>
                                                                  !existingCurrency.Contains(currencyData.DetailsId))
                                                       .ToArray();
        await applicationDbContext.Currency.AddRangeAsync(
            newPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        _logger.LogInformation("Added {Result} new Currency", newPoeNinjaCurrencyData.Length);

        var updatedPoeNinjaCurrencyData =
            currencyPriceData.Lines.Where(gem => existingCurrency.Contains(gem.DetailsId)).ToArray();
        applicationDbContext.Currency.UpdateRange(
            updatedPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        _logger.LogInformation("Updated {Result} Currency", updatedPoeNinjaCurrencyData.Length);

        #region Chaos

        // not EqualsIgnoreCase because of EntityFramework
        var chaos = applicationDbContext.Currency
                                        .FirstOrDefault(currency => currency.Name.ToLower().Equals("chaos orb"));
        if (chaos is not null) return;
        await applicationDbContext.Currency.AddAsync(new Currency
                                                     {
                                                         Name = "Chaos Orb",
                                                         ChaosEquivalent = 1,
                                                         Icon =
                                                             "https://web.poecdn.com/image/Art/2DItems/Currency/CurrencyRerollRare.png",
                                                         Id = "chaos-orb"
                                                     });
        _logger.LogInformation("Saved Chaos Orb");

        #endregion

        await applicationDbContext.SaveChangesAsync();
    }

    public async Task FetchTemplePriceData(League league) { Console.WriteLine("NOT IMPLEMENTED"); }

    public async Task FetchGemPriceData(League league) { Console.WriteLine("NOT IMPLEMENTED"); }

    #region Helper Methods

    private static (int versionColumn, int nameColumn, int releaseColumn) GetIndexesFromTitleRow(HtmlNode titleRow)
    {
        var versionColumn = titleRow.ChildNodes.First(td => td.InnerText.EqualsIgnoreCase("version"));
        if (versionColumn is null) throw new PoeDbCannotParseException("no version column in league table");
        var versionColumnIndex = titleRow.ChildNodes.IndexOf(versionColumn);

        var nameColumn = titleRow.ChildNodes.First(td => td.InnerText.EqualsIgnoreCase("league"));
        if (nameColumn is null) throw new PoeDbCannotParseException("no name column in league table");
        var nameColumnIndex = titleRow.ChildNodes.IndexOf(nameColumn);

        var releaseColumn = titleRow.ChildNodes.First(td => td.InnerText.EqualsIgnoreCase("international"));
        if (releaseColumn is null) throw new PoeDbCannotParseException("no release column in league table");
        var releaseColumnIndex = titleRow.ChildNodes.IndexOf(releaseColumn);

        return (versionColumnIndex, nameColumnIndex, releaseColumnIndex);
    }

    private async Task ParseLeagueRows(IEnumerable<HtmlNode> leagueRows,
                                       int releaseColumnIndex,
                                       int nameColumnIndex,
                                       int versionColumnIndex)
    {
        var yearRegex = YearRegex();
        var fullDateRegex = FullDateRegex();
        var nameExpansionRegex = NameExpansionRegex();

        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        foreach (var row in leagueRows)
        {
            var releaseDateText = row.ChildNodes[releaseColumnIndex].InnerText;
            var date = DateTime.SpecifyKind(
                yearRegex.IsMatch(releaseDateText)
                    ? new DateTime(int.Parse(releaseDateText), 12, 31)
                    : fullDateRegex.IsMatch(releaseDateText)
                        ? DateTime.Parse(releaseDateText)
                        : DateTime.MaxValue,
                DateTimeKind.Utc
            );

            var name = nameExpansionRegex.Replace(row.ChildNodes[nameColumnIndex].InnerText, "").Trim();
            var version = row.ChildNodes[versionColumnIndex].InnerText;

            // not EqualsIgnoreCase because of EntityFramework
            var dbLeague = applicationDbContext.League
                                               .FirstOrDefault(
                                                   dbLeague => dbLeague.Version.ToLower().Equals(version.ToLower())
                                               );

            if (dbLeague is null)
            {
                var league = new League { Name = name, StartDate = date, Version = version };
                await applicationDbContext.League.AddAsync(league);
                _logger.LogInformation("Saved League: {League}", league);
                continue;
            }

            dbLeague.Name = name;
            dbLeague.StartDate = date;
            dbLeague.Version = version;
            applicationDbContext.League.Update(dbLeague);
            _logger.LogInformation("Updated League: {League}", dbLeague);
        }

        await applicationDbContext.SaveChangesAsync();
    }

    [GeneratedRegex("^\\d\\d\\d\\d$")] private static partial Regex YearRegex();
    [GeneratedRegex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$")] private static partial Regex FullDateRegex();
    [GeneratedRegex("&lt;.+&gt;")] private static partial Regex NameExpansionRegex();

    #endregion
}