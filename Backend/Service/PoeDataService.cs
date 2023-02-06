using Shared.Entity;

namespace Backend.Service;

public class PoeDataService : Service, IPoeDataService
{
    private readonly IRepository<League, Guid> _leagueRepository;

    #region Con- and Destruction

    public PoeDataService(ILogger<PoeDataService> logger, IServiceScopeFactory factory) : base(logger, factory)
    {
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League, Guid>>();
    }

    #endregion

    #region public methods

    public League? GetCurrentLeague()
    {
        return _leagueRepository
               .GetAll(leagues => leagues.Where(league => DateTime.Now.ToUniversalTime() >= league.StartDate))
               .MaxBy(league => league.StartDate);
    }

    #endregion
}