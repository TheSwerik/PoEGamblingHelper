using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;
    public LeagueService(ILeagueRepository leagueRepository) { _leagueRepository = leagueRepository; }

    public League GetCurrent()
    {
        var utcNow = DateTime.Today.ToUniversalTime(); //TODO DateTime Today in Infrastructure
        return _leagueRepository.GetByStartDateAfter(utcNow);
    }

    public IAsyncEnumerable<League> GetAll() { return _leagueRepository.GetAllLeagues(); }
}