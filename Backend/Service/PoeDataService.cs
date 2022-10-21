using Backend.Model;

namespace Backend.Service;

public class PoeDataService : Service, IPoeDataService
{
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
                .GetAll(leagues => leagues.Where(league => league.StartDate >= DateTime.Now.ToUniversalTime()))
                .OrderBy(league => league.StartDate)
                .First()
        );
    }

    #endregion
}