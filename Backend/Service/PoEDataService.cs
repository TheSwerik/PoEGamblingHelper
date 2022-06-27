using Backend.Model;

namespace Backend.Service;

public class PoEDataService : Service, IPoEDataService
{
    //https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
    //https://www.pathofexile.com/developer/docs/reference#leagues
    private const string PoeApiUrl = "https://api.pathofexile.com";
    private const string PoeNinjaUrl = "https://poe.ninja/api/data";
    private readonly HttpClient _client = new();
    private readonly IRepository<League> _leagueRepository;

    public PoEDataService(ILogger<PoEDataService> logger, IRepository<Gem> gemRepository,
                          IRepository<League> leagueRepository)
        : base(logger, gemRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task GetCurrentLeague()
    {
        //TODO check current league
        //TODO get next league if next league is released
        //TODO if new league, clear db
        const string leagueUrl = PoeApiUrl + "/league";
        var result = await _client.GetStringAsync(PoeApiUrl);
        _logger.LogInformation("{Result}", result);
        await _leagueRepository.Save(new League());
    }

    public async Task GetPriceData()
    {
        const string gemUrl = PoeNinjaUrl + "/itemoverview?league=Sentinel&type=SkillGem";
        var result = await _client.GetStringAsync(PoeApiUrl);
        _logger.LogInformation("{Result}", result);
        await _gemRepository.Save(new[] { new Gem() });
    }

    public void Dispose() { _client.Dispose(); }
}