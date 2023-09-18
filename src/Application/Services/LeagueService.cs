using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public class LeagueService : ILeagueService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILeagueRepository _leagueRepository;

    public LeagueService(ILeagueRepository leagueRepository, IDateTimeService dateTimeService)
    {
        _leagueRepository = leagueRepository;
        _dateTimeService = dateTimeService;
    }

    public League GetCurrent() { return _leagueRepository.GetByStartDateBefore(_dateTimeService.UtcNow()); }

    public IAsyncEnumerable<League> GetAll() { return _leagueRepository.GetAllLeagues(); }
}