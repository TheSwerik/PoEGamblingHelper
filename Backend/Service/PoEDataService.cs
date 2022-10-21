using System.Text.RegularExpressions;
using Backend.Model;
using HtmlAgilityPack;

namespace Backend.Service;

public class PoEDataService : Service, IPoEDataService
{
    //https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
    //https://www.pathofexile.com/developer/docs/reference#leagues
    private const string PoeApiUrl = "https://api.pathofexile.com";
    private const string PoeWikiUrl = "https://pathofexile.fandom.com/wiki";
    private const string PoeDbUrl = "https://poedb.tw/us/";
    private const string PoeNinjaUrl = "https://poe.ninja/api/data";
    private readonly HttpClient _client = new();
    private readonly IRepository<Gem> _gemRepository;
    private readonly IRepository<League> _leagueRepository;

    public PoEDataService(
        ILogger<PoEDataService> logger,
        IServiceScopeFactory factory
    )
        : base(logger, factory)
    {
        _gemRepository = Scope.ServiceProvider.GetRequiredService<IRepository<Gem>>();
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League>>();
    }

    public async Task GetCurrentLeague()
    {
        var web = new HtmlWeb();
        var doc = web.Load(PoeDbUrl + "League#LeaguesList");
        if (doc is null) throw new NullReferenceException("poedb is down");
        var leaguesTable = doc.DocumentNode.SelectNodes("//table").First(n => n.HasChildNodes);
        var leagues = leaguesTable.SelectNodes(".//tr").Where(n => n.HasChildNodes).ToArray();
        var titleRow = leagues[0];
        if (titleRow is null) throw new NullReferenceException("No rows found");
        var (versionColumn, nameColumn, releaseColumn) = GetIndexesFromTitleRow(titleRow);

        var yearRegex = new Regex("^\\d\\d\\d\\d$");
        var fullDateRegex = new Regex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$");
        var nameExpansionRegex = new Regex("&lt;.+&gt;");
        foreach (var row in leagues.Skip(1).Where(row => row.HasChildNodes))
        {
            var date = DateTime.MaxValue;
            if (yearRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = new DateTime(int.Parse(row.ChildNodes[releaseColumn].InnerText), 12, 31);
            else if (fullDateRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = DateTime.Parse(row.ChildNodes[releaseColumn].InnerText);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var name = nameExpansionRegex.Replace(row.ChildNodes[nameColumn].InnerText, "").Trim();
            var version = row.ChildNodes[versionColumn].InnerText;

            var dbLeague = _leagueRepository
                           .GetAll()
                           .FirstOrDefault(dbLeague => dbLeague.Version.Equals(
                                               version,
                                               StringComparison.InvariantCultureIgnoreCase
                                           ));

            if (dbLeague is not null)
            {
                dbLeague.Name = name;
                dbLeague.StartDate = date;
                dbLeague.Version = version;
                Logger.LogInformation("Updated League: {League}", dbLeague);
                await _leagueRepository.Update(dbLeague);
                continue;
            }

            var league = new League { Name = name, StartDate = date, Version = version };
            Logger.LogInformation("Saved League: {League}", league);
            await _leagueRepository.Save(league);
        }
    }

    public async Task GetPriceData()
    {
        const string gemUrl = PoeNinjaUrl + "/itemoverview?type=SkillGem";
        var currentLeague = _leagueRepository.GetAll()
                                             .Where(league => league.StartDate <= DateTime.UtcNow.Date)
                                             .OrderBy(league => league.StartDate)
                                             .Select(league => league.Name)
                                             .First();
        var result = await _client.GetFromJsonAsync<PriceData>(gemUrl + $"&league={currentLeague}");
        if (result is null) throw new NullReferenceException();
        Logger.LogInformation("Got data from {Result} gems", result.Lines.Length);
        var trackedGems = _gemRepository.GetAll().Select(gem => gem.Id).ToArray();
        await _gemRepository.Save(result.Lines.Where(gem => !trackedGems.Contains(gem.Id)));
        await _gemRepository.Update(result.Lines.Where(gem => !trackedGems.Contains(gem.Id)));
    }

    public new void Dispose()
    {
        base.Dispose();
        _client.Dispose();
    }

    private (int versionColumn, int nameColumn, int releaseColumn) GetIndexesFromTitleRow(HtmlNode titleRow)
    {
        var versionColumn =
            titleRow.ChildNodes.First(
                td => td.InnerText.Equals("version", StringComparison.InvariantCultureIgnoreCase));
        if (versionColumn is null) throw new NullReferenceException("no version column");
        var versionColumnIndex = titleRow.ChildNodes.IndexOf(versionColumn);

        var nameColumn =
            titleRow.ChildNodes.First(td => td.InnerText.Equals("league", StringComparison.InvariantCultureIgnoreCase));
        if (nameColumn is null) throw new NullReferenceException("no name column");
        var nameColumnIndex = titleRow.ChildNodes.IndexOf(nameColumn);

        var releaseColumn =
            titleRow.ChildNodes.First(
                td => td.InnerText.Equals("international", StringComparison.InvariantCultureIgnoreCase));
        if (releaseColumn is null) throw new NullReferenceException("no release column");
        var releaseColumnIndex = titleRow.ChildNodes.IndexOf(releaseColumn);

        return (versionColumnIndex, nameColumnIndex, releaseColumnIndex);
    }
}