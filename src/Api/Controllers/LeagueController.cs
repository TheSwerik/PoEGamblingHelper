using Domain.Entity;
using Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService) { _leagueService = leagueService; }

    [HttpGet] public IAsyncEnumerable<League> GetAllLeagues() { return _leagueService.GetAllAsync(); }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague() { return _leagueService.GetCurrentLeague() ?? throw new NoLeagueDataException(); }
}