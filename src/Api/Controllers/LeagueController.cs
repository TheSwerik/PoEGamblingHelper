using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly ILeagueRepository _leagueRepository;

    public LeagueController(ILeagueRepository leagueRepository) { _leagueRepository = leagueRepository; }

    [HttpGet] public IAsyncEnumerable<League> GetAll() { return _leagueRepository.GetAllLeagues(); }

    [HttpGet("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrent() { return _leagueRepository.GetCurrent(); }
}