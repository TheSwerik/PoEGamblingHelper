using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;
    public LeagueService(ILeagueRepository leagueRepository) { _leagueRepository = leagueRepository; }

    public League GetCurrentLeague()
    {
        var utcNow = DateTime.Today.ToUniversalTime();
        return _leagueRepository.GetByStartDateAfter(utcNow);
        // return leagues.Where(league => utcNow >= league.StartDate)
        // .OrderByDescending(league => league.StartDate)
        // .FirstOrDefault()
        // ?? throw new NoLeagueDataException();
    }
}