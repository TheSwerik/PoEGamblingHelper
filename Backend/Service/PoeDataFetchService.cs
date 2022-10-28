using System.Text.RegularExpressions;
using Backend.Model;
using HtmlAgilityPack;

namespace Backend.Service;

public class PoeDataFetchService : Service, IPoeDataFetchService
{
    //https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
    //https://www.pathofexile.com/developer/docs/reference#leagues
    private const string PoeApiUrl = "https://api.pathofexile.com";
    private const string PoeWikiUrl = "https://pathofexile.fandom.com/wiki";
    private const string PoeDbUrl = "https://poedb.tw/us/";
    private const string PoeNinjaUrl = "https://poe.ninja/api/data";
    private const string PoeTradeUrl = "https://www.pathofexile.com/trade/search/";
    private static readonly Regex WhitespaceRegex = new("\\s");
    private readonly HttpClient _client = new();
    private readonly IRepository<GemData> _gemDataRepository;
    private readonly IRepository<League> _leagueRepository;
    private readonly IPoeDataService _poeDataService;

    #region Helper Methods

    private static (int versionColumn, int nameColumn, int releaseColumn) GetIndexesFromTitleRow(HtmlNode titleRow)
    {
        var versionColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("version", StringComparison.InvariantCultureIgnoreCase)
        );
        if (versionColumn is null) throw new NullReferenceException("no version column");
        var versionColumnIndex = titleRow.ChildNodes.IndexOf(versionColumn);

        var nameColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("league", StringComparison.InvariantCultureIgnoreCase)
        );
        if (nameColumn is null) throw new NullReferenceException("no name column");
        var nameColumnIndex = titleRow.ChildNodes.IndexOf(nameColumn);

        var releaseColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("international", StringComparison.InvariantCultureIgnoreCase)
        );
        if (releaseColumn is null) throw new NullReferenceException("no release column");
        var releaseColumnIndex = titleRow.ChildNodes.IndexOf(releaseColumn);

        return (versionColumnIndex, nameColumnIndex, releaseColumnIndex);
    }

    #endregion

    #region Con- and Destruction

    public PoeDataFetchService(ILogger<PoeDataFetchService> logger, IServiceScopeFactory factory) : base(
        logger, factory)
    {
        _gemDataRepository = Scope.ServiceProvider.GetRequiredService<IRepository<GemData>>();
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League>>();
        _poeDataService = Scope.ServiceProvider.GetRequiredService<IPoeDataService>();
    }

    public new void Dispose()
    {
        base.Dispose();
        _client.Dispose();
    }

    #endregion

    #region public methods

    public async Task GetCurrentLeague()
    {
        var web = new HtmlWeb();
        var doc = web.Load(PoeDbUrl + "League#LeaguesList");
        if (doc is null) throw new NullReferenceException("PoeDB is down");

        var leaguesTable = doc.DocumentNode.SelectNodes("//table").First(n => n.HasChildNodes);
        if (leaguesTable is null) throw new NullReferenceException("No tables found");

        var leagues = leaguesTable.SelectNodes(".//tr").Where(n => n.HasChildNodes).ToArray();
        if (leagues.Length == 0) throw new NullReferenceException("No rows found");
        var titleRow = leagues[0];

        var (versionColumn, nameColumn, releaseColumn) = GetIndexesFromTitleRow(titleRow);

        var yearRegex = new Regex("^\\d\\d\\d\\d$");
        var fullDateRegex = new Regex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$");
        var nameExpansionRegex = new Regex("&lt;.+&gt;");
        foreach (var row in leagues.Skip(1).Where(row => row.HasChildNodes))
        {
            #region date

            var date = DateTime.MaxValue;
            if (yearRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = new DateTime(int.Parse(row.ChildNodes[releaseColumn].InnerText), 12, 31);
            else if (fullDateRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = DateTime.Parse(row.ChildNodes[releaseColumn].InnerText);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            #endregion

            var name = nameExpansionRegex.Replace(row.ChildNodes[nameColumn].InnerText, "").Trim();
            var version = row.ChildNodes[versionColumn].InnerText;

            var dbLeague = _leagueRepository
                           .GetAll()
                           .FirstOrDefault(
                               dbLeague => dbLeague.Version.Equals(version, StringComparison.InvariantCultureIgnoreCase)
                           );

            if (dbLeague is null)
            {
                var league = new League { Name = name, StartDate = date, Version = version };
                Logger.LogInformation("Saved League: {League}", league);
                await _leagueRepository.Save(league);
                continue;
            }

            dbLeague.Name = name;
            dbLeague.StartDate = date;
            dbLeague.Version = version;
            Logger.LogInformation("Updated League: {League}", dbLeague);
            await _leagueRepository.Update(dbLeague);
        }
    }

    public async Task GetPriceData()
    {
        const string gemUrl = PoeNinjaUrl + "/itemoverview?type=SkillGem";
        var currentLeague = await _poeDataService.GetCurrentLeague();
        var result = await _client.GetFromJsonAsync<PriceData>(gemUrl + $"&league={currentLeague.Name}");
        if (result is null) throw new NullReferenceException();
        Logger.LogInformation("Got data from {Result} gems", result.Lines.Length);

        var trackedGems = _gemDataRepository.GetAll().Select(gem => gem.Id).ToArray();
        _gemDataRepository.ClearTrackedEntities();

        var newGems = result.Lines.Where(gem => !trackedGems.Contains(gem.Id)).ToArray();
        await _gemDataRepository.Save(newGems);
        Logger.LogInformation("Added {Result} new gems", newGems.Length);
        var updatedGems = result.Lines.Where(gem => trackedGems.Contains(gem.Id)).ToArray();
        await _gemDataRepository.Update(updatedGems);
        Logger.LogInformation("Updated {Result} gems", updatedGems.Length);
    }

    public async Task<string> GetPoeTradeUrl(Gem gem, bool accurateGemLevel = false, bool accurateGemQuality = false)
    {
        const string queryKey = "?q=";
        var currentLeague = await _poeDataService.GetCurrentLeague();
        Console.WriteLine(PoeTradeUrl + currentLeague.Name + queryKey + WhitespaceRegex.Replace(gem.TradeQuery(), ""));
        Console.WriteLine(PoeTradeUrl + currentLeague.Name + queryKey +
                          WhitespaceRegex.Replace(gem.TradeQuery(true, true), ""));
        return PoeTradeUrl + currentLeague.Name + queryKey +
               WhitespaceRegex.Replace(gem.TradeQuery(accurateGemLevel, accurateGemQuality), "");
    }

    #endregion
}