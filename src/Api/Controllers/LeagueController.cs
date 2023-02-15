using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ILeagueService _leagueService;

    public LeagueController(IApplicationDbContext applicationDbContext, ILeagueService leagueService)
    {
        _applicationDbContext = applicationDbContext;
        _leagueService = leagueService;
    }

    [HttpGet]
    public IAsyncEnumerable<League> GetAllLeagues() { return _applicationDbContext.League.AsAsyncEnumerable(); }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague() { return _leagueService.GetCurrentLeague(_applicationDbContext); }
}