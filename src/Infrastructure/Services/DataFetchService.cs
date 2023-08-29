using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Services.FetchDtos;
using PoEGamblingHelper.Infrastructure.Util;

namespace PoEGamblingHelper.Infrastructure.Services;

public partial class DataFetchService : IDataFetchService, IDisposable
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly HtmlWeb _htmlLoader = new();
    private readonly HttpClient _httpClient = new();
    private readonly MediaTypeHeaderValue _jsonMediaTypeHeader = MediaTypeHeaderValue.Parse("application/json");
    private readonly ILogger<DataFetchService> _logger;
    private readonly string _templeQuery;

    public DataFetchService(ILogger<DataFetchService> logger, IApplicationDbContextFactory applicationDbContextFactory)
    {
        _logger = logger;
        _applicationDbContextFactory = applicationDbContextFactory;
        _httpClient.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("PoEGamblingHelper/1.0.0"));
        _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        _templeQuery = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/TempleQuery.json");
    }

    public async Task FetchCurrentLeague()
    {
        HtmlDocument doc;
        try
        {
            doc = _htmlLoader.Load(PoeToolUrls.PoeDbLeagueUrl);
        }
        catch (WebException e)
        {
            throw new ApiDownException(PoeToolUrls.PoeDbLeagueUrl, e.Message);
        }

        if (doc is null) throw new ApiDownException(PoeToolUrls.PoeDbLeagueUrl);
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
        var response = await GetAsync($"{PoeToolUrls.PoeNinjaCurrencyUrl}&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeNinjaCurrencyUrl);
        var currencyPriceData = await response.Content.ReadFromJsonAsync<CurrencyPriceData>();
        if (currencyPriceData is null) throw new ApiDownException(PoeToolUrls.PoeNinjaCurrencyUrl);
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

        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
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

        await applicationDbContext.SaveChangesAsync();

        #region Hardcoded Chaos Orb

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

    public async Task FetchTemplePriceData(League league)
    {
        #region Fetch Temple Fetch

        _logger.LogDebug("Fetching Temple IDs...");

        TradeResults temples;
        var requestUri = $"{PoeToolUrls.PoeApiTradeUrl}/search/{league.Name}";
        using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
                             { Content = new StringContent(_templeQuery, _jsonMediaTypeHeader) })
        {
            var result = await SendAsync(request);
            if (!result.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);

            temples = await result.Content.ReadFromJsonAsync<TradeResults>() ??
                      throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);
        }

        _logger.LogDebug("Found {ResultLength} Temples IDs", temples.Result.Length);

        #endregion

        #region Fetch Temples

        var takeAmount = Math.Min(10, temples.Result.Length);
        var skipAmount = takeAmount == temples.Result.Length ? 0 : 2;
        var itemQuery = string.Join(",", temples.Result.Skip(skipAmount).Take(takeAmount));

        _logger.LogDebug("Skipping {SkipAmount} and fetching {TakeAmount} Temples...", skipAmount, takeAmount);

        TradeEntryResult priceResults;
        requestUri = $"{PoeToolUrls.PoeApiTradeUrl}/fetch/{itemQuery}?query={temples.Id}";
        using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri)
                             { Content = new StringContent(_templeQuery, _jsonMediaTypeHeader) })
        {
            var result = await SendAsync(request);
            if (!result.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);

            priceResults = await result.Content.ReadFromJsonAsync<TradeEntryResult>()
                           ?? throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);
        }

        _logger.LogDebug("Found {ResultLength} TemplePrices", priceResults.Result.Length);

        #endregion

        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        var chaosValues = priceResults.Result
                                      .Select(priceResult =>
                                                  priceResult.Listing
                                                             .Price
                                                             .ChaosAmount(applicationDbContext.Currency)
                                      )
                                      .ToArray();

        await applicationDbContext.TempleCost.ExecuteDeleteAsync(); // Delete every Temple Entry
        await applicationDbContext.TempleCost.AddAsync(new TempleCost { ChaosValue = chaosValues });
        await applicationDbContext.SaveChangesAsync();

        _logger.LogInformation("Saved {PriceLength} TemplePrices", chaosValues.Length);
    }

    public async Task FetchGemPriceData(League league)
    {
        var response = await GetAsync(PoeToolUrls.PoeNinjaGemUrl + $"&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeNinjaGemUrl);
        var gemPriceData = await response.Content.ReadFromJsonAsync<GemPriceData>();
        if (gemPriceData is null) throw new ApiDownException(PoeToolUrls.PoeNinjaGemUrl);
        _logger.LogInformation("Got data from {Result} gems", gemPriceData.Lines.Length);

        // GemTradeData
        using (var applicationDbContext = _applicationDbContextFactory.CreateDbContext())
        {
            var existingGemTradeData =
                applicationDbContext.GemTradeData.Select(gemTradeData => gemTradeData.Id).ToArray();
            applicationDbContext.ClearTrackedEntities();

            var newGemTradeData = gemPriceData.Lines.Where(gem => !existingGemTradeData.Contains(gem.Id)).ToArray();
            await applicationDbContext.GemTradeData.AddRangeAsync(
                newGemTradeData.Select(gemTradeData => gemTradeData.ToGemTradeData()));
            _logger.LogInformation("Added {Result} new GemTradeData", newGemTradeData.Length);

            var updatedGemTradeData = gemPriceData.Lines.Where(gem => existingGemTradeData.Contains(gem.Id)).ToArray();
            applicationDbContext.GemTradeData.UpdateRange(
                updatedGemTradeData.Select(gemTradeData => gemTradeData.ToGemTradeData()));
            _logger.LogInformation("Updated {Result} GemTradeData", updatedGemTradeData.Length);
            await applicationDbContext.SaveChangesAsync();
        }

        // GemData
        using (var applicationDbContext = _applicationDbContextFactory.CreateDbContext())
        {
            var existingGemData = applicationDbContext.GemData.ToArray();
            var existingGemDataNames = existingGemData.Select(gem => gem.Name.ToLowerInvariant().Trim()).ToArray();
            applicationDbContext.ClearTrackedEntities();

            var allGemTradeData = applicationDbContext.GemTradeData.ToArray();
            var groupedPoeNinjaData = gemPriceData.Lines.GroupBy(priceData => priceData.Name).ToArray();

            var newGemData = groupedPoeNinjaData
                             .Where(group => !existingGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                             .ToArray();
            await applicationDbContext.GemData.AddRangeAsync(
                newGemData.Select(group => group.ToGemData(allGemTradeData)));
            _logger.LogInformation("Added {Result} GemData", newGemData.Length);

            var updatedGemData = groupedPoeNinjaData
                                 .Where(group => existingGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                                 .ToArray();
            applicationDbContext.GemData.UpdateRange(
                updatedGemData.Select(group => group.ToGemData(allGemTradeData, existingGemData))
            );
            _logger.LogInformation("Updated {Result} GemData", updatedGemData.Length);

            await applicationDbContext.SaveChangesAsync();
        }
    }

    public void Dispose() { _httpClient.Dispose(); }

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

    private async Task<HttpResponseMessage> GetAsync(string url)
    {
        try
        {
            return await _httpClient.GetAsync(url);
        }
        catch (HttpRequestException e)
        {
            throw new ApiDownException(url, e.Message);
        }
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        try
        {
            return await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException e)
        {
            throw new ApiDownException(request.RequestUri!.Host, e.Message);
        }
    }

    [GeneratedRegex("^\\d\\d\\d\\d$")] private static partial Regex YearRegex();

    [GeneratedRegex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$")] private static partial Regex FullDateRegex();

    [GeneratedRegex("&lt;.+&gt;")] private static partial Regex NameExpansionRegex();

    #endregion
}