using System.Text.RegularExpressions;
using Model;

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

    public async Task<string> GetPoeTradeUrl(GemData gem, GemTradeData gemTradeData, bool accurateGemLevel = false,
                                             bool accurateGemQuality = false)
    {
        const string queryKey = "?q=";
        var currentLeague = await GetCurrentLeague();
        var query = WhitespaceRegex.Replace(gemTradeData.TradeQuery(gem.Name, accurateGemLevel, accurateGemQuality),
                                            "");
        Console.WriteLine(PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey +
                          WhitespaceRegex.Replace(gemTradeData.TradeQuery(gem.Name), ""));
        Console.WriteLine(PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey +
                          WhitespaceRegex.Replace(gemTradeData.TradeQuery(gem.Name, true, true), ""));
        return PoeToolUrls.PoeTradeUrl + currentLeague.Name + queryKey + query;
    }

    #endregion
}