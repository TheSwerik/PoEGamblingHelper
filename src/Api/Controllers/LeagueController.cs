using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService) { _leagueService = leagueService; }

    [HttpGet] public IAsyncEnumerable<League> GetAll() { return _leagueService.GetAll(); }

    [HttpGet("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrent() { return _leagueService.GetCurrent(); }
}