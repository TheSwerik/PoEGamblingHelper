using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public partial class LeagueDataFetcher(ILogger logger,
                                       IDbContextFactory<ApplicationDbContext> applicationDbContextFactory)
    : ILeagueDataFetcher
{
    private static readonly Regex Regex1 = new Regex("^\\d\\d\\d\\d$");
    private readonly HtmlWeb _htmlLoader = new();

    public async Task Fetch()
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

        await ParseLeagueRows(
            leagueRows.Skip(1).Where(row => row.HasChildNodes),
            GetColumnIndexFromTitleRow(titleRow, "international"),
            GetColumnIndexFromTitleRow(titleRow, "league"),
            GetColumnIndexFromTitleRow(titleRow, "version")
        );
    }

    #region Helper Methods

    private static int GetColumnIndexFromTitleRow(HtmlNode titleRow, string column)
    {
        var result = titleRow.ChildNodes.First(td => td.InnerText.EqualsIgnoreCase(column));
        if (result is null) throw new PoeDbCannotParseException($"no {column} column in league table");
        return titleRow.ChildNodes.IndexOf(result);
    }

    private async Task ParseLeagueRows(IEnumerable<HtmlNode> leagueRows,
                                       int releaseColumnIndex,
                                       int nameColumnIndex,
                                       int versionColumnIndex)
    {
        await using var applicationDbContext = await applicationDbContextFactory.CreateDbContextAsync();

        foreach (var row in leagueRows)
        {
            var releaseDateText = row.ChildNodes[releaseColumnIndex].InnerText;
            var date = DateTime.SpecifyKind(
                YearRegex.IsMatch(releaseDateText)
                    ? new DateTime(int.Parse(releaseDateText), 12, 31)
                    : FullDateRegex.IsMatch(releaseDateText)
                        ? DateTime.Parse(releaseDateText)
                        : DateTime.MaxValue,
                DateTimeKind.Utc
            );

            var name = NameExpansionRegex.Replace(row.ChildNodes[nameColumnIndex].InnerText, "").Trim();
            var version = row.ChildNodes[versionColumnIndex].InnerText;

            // not EqualsIgnoreCase because of EntityFramework
            var dbLeague = applicationDbContext.League
                                               .FirstOrDefault(
                                                   dbLeague => dbLeague.Version.ToLower().Equals(version.ToLower()));

            if (dbLeague is null)
            {
                var league = new League { Name = name, StartDate = date, Version = version };
                await applicationDbContext.League.AddAsync(league);
                logger.LogInformation("Saved League: {League}", league);
                continue;
            }

            dbLeague.Name = name;
            dbLeague.StartDate = date;
            dbLeague.Version = version;
            applicationDbContext.League.Update(dbLeague);
            logger.LogInformation("Updated League: {League}", dbLeague);
        }

        await applicationDbContext.SaveChangesAsync();
    }

    private static readonly Regex YearRegex = new(@"^\d\d\d\d$");
    private static readonly Regex FullDateRegex = new(@"^\d\d\d\d-\d\d-\d\d$");
    private static readonly Regex NameExpansionRegex = new("&lt;.+&gt;");

    #endregion
}