using System.Text.RegularExpressions;
using Application.Services;
using Domain.Entity;
using Domain.Exception;
using HtmlAgilityPack;
using Infrastructure.Util;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public partial class DataFetchService : IDataFetchService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly HtmlWeb _htmlLoader = new();
    private readonly ILeagueService _leagueService;
    private readonly ILogger<DataFetchService> _logger;

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

    public Task FetchCurrencyData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }

    public Task FetchTemplePriceData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }

    public Task FetchGemPriceData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }

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

            var dbLeague = _applicationDbContext.League
                                                .FirstOrDefault(dbLeague => dbLeague.Version.EqualsIgnoreCase(version));

            if (dbLeague is null)
            {
                var league = new League { Name = name, StartDate = date, Version = version };
                await _applicationDbContext.League.AddAsync(league);
                _logger.LogInformation("Saved League: {League}", league);
                continue;
            }

            dbLeague.Name = name;
            dbLeague.StartDate = date;
            dbLeague.Version = version;
            _applicationDbContext.League.Update(dbLeague);
            _logger.LogInformation("Updated League: {League}", dbLeague);
        }

        await _applicationDbContext.SaveChangesAsync();
    }

    [GeneratedRegex("^\\d\\d\\d\\d$")] private static partial Regex YearRegex();
    [GeneratedRegex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$")] private static partial Regex FullDateRegex();
    [GeneratedRegex("&lt;.+&gt;")] private static partial Regex NameExpansionRegex();

    #endregion
}