using System.Text.RegularExpressions;
using Backend.Model;

namespace Backend.Service;

public class PoeDataService : Service, IPoeDataService
{
    private static readonly Regex WhitespaceRegex = new("\\s");
    private readonly IRepository<League> _leagueRepository;

    #region Con- and Destruction

    public PoeDataService(ILogger<PoeDataService> logger, IServiceScopeFactory factory) : base(logger, factory)
    {
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League>>();
    }

    #endregion

    #region public methods

    public Task<League> GetCurrentLeague()
    {
        return Task.FromResult(
            _leagueRepository
                .GetAll(leagues => leagues.Where(league => DateTime.Now.ToUniversalTime() >= league.StartDate))
                .OrderByDescending(league => league.StartDate)
                .First()
        );
    }

    public async Task<string> GetPoeTradeUrl(Gem gem, bool accurateGemLevel = false, bool accurateGemQuality = false)
    {
        const string queryKey = "?q=";
        var currentLeague = await GetCurrentLeague();
        var query = WhitespaceRegex.Replace(gem.TradeQuery(accurateGemLevel, accurateGemQuality), "");
        Console.WriteLine(PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey +
                          WhitespaceRegex.Replace(gem.TradeQuery(), ""));
        Console.WriteLine(PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey +
                          WhitespaceRegex.Replace(gem.TradeQuery(true, true), ""));
        return PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey + query;
    }

    #endregion
}