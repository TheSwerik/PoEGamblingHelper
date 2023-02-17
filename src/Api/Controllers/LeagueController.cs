using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService, IApplicationDbContextFactory applicationDbContextFactory)
    {
        _leagueService = leagueService;
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    [HttpGet]
    public IAsyncEnumerable<League> GetAllLeagues()
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        return applicationDbContext.League.AsAsyncEnumerable();
    }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague()
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        return _leagueService.GetCurrentLeague(applicationDbContext.League);
    }
}