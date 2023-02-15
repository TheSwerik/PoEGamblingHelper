using Domain.Entity;
using Domain.Exception;

namespace Application.Services;

public class LeagueService : ILeagueService
{
    public League GetCurrentLeague(IApplicationDbContext applicationDbContext)
    {
        var utcNow = DateTime.Now.ToUniversalTime();
        return applicationDbContext.League
                                   .Where(league => utcNow >= league.StartDate)
                                   .MaxBy(league => league.StartDate)
               ?? throw new NoLeagueDataException();
    }
}