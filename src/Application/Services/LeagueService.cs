using Domain.Entity;
using Domain.Exception;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LeagueService : ILeagueService
{
    public League GetCurrentLeague(DbSet<League> leagues)
    {
        var utcNow = DateTime.Today.ToUniversalTime();
        return leagues.Where(league => utcNow >= league.StartDate)
                      .OrderByDescending(league => league.StartDate)
                      .FirstOrDefault()
               ?? throw new NoLeagueDataException();
    }
}