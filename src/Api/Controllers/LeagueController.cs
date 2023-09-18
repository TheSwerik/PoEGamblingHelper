using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Api.Extensions;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService, IAnalyticsService analyticsService)
    {
        _leagueService = leagueService;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    public IAsyncEnumerable<League> GetAllLeagues() //TODO
    {
        _analyticsService.AddView(Request.GetRealIpAddress()).RunSynchronously();
        return _leagueService.GetAllLeagues();
    }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague()
    {
        _analyticsService.AddView(Request.GetRealIpAddress());
        return _leagueService.GetCurrentLeague();
    }
}