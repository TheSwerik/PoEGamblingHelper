using Backend.Model;
using HtmlAgilityPack;

namespace Backend.Service;

public class PoEDataService : Service, IPoEDataService
{
    //https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
    //https://www.pathofexile.com/developer/docs/reference#leagues
    private const string PoeApiUrl = "https://api.pathofexile.com";
    private const string PoeWikiUrl = "https://pathofexile.fandom.com/wiki";
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
        var doc = web.Load(PoeWikiUrl + "/League");
        if (doc is null) throw new NullReferenceException("wiki is down");
        var currentLeague = doc.DocumentNode.SelectNodes("//tr/td")
                               .Where(n => n.HasChildNodes)
                               .Select(n => n.FirstChild.InnerHtml)
                               .First();
        var currentStartDate = doc.DocumentNode.SelectNodes("//tr/td")
                                  .Where(n => n.HasChildNodes)
                                  .Select(n => n.FirstChild.InnerHtml)
                                  .Skip(1)
                                  .First();
        if (currentLeague is null) throw new NullReferenceException("No current league found");
        if (currentStartDate is null) throw new NullReferenceException("No current league start date found");
        if (currentLeague == "Lake of Kalandra") currentLeague = "Kalandra";
        await _leagueRepository.Save(
            new League { Name = currentLeague, StartDate = DateTime.Parse(currentStartDate) });
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
        Logger.LogInformation("Got {Result} pricedata lines", result.Lines.Length);
        var trackedGems = _gemRepository.GetAll().Select(gem => gem.Id).ToArray();
        await _gemRepository.Save(result.Lines.Where(gem => !trackedGems.Contains(gem.Id)));
        await _gemRepository.Update(result.Lines.Where(gem => !trackedGems.Contains(gem.Id)));
    }

    public new void Dispose()
    {
        base.Dispose();
        _client.Dispose();
    }
}