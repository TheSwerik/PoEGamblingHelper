using Domain.Entity;
using Domain.Exception;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LeagueService : ILeagueService
{
    public League GetCurrentLeague(DbSet<League> leagues)
    {
        var utcNow = DateTime.Now.ToUniversalTime();
        return leagues.Where(league => utcNow >= league.StartDate)
                      .OrderByDescending(league => league.StartDate)
                      .FirstOrDefault()
               ?? throw new NoLeagueDataException();
    }
}