using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class LeagueController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly ILeagueService _leagueService;

    public LeagueController(ILeagueService leagueService,
                            IApplicationDbContextFactory applicationDbContextFactory,
                            IAnalyticsService analyticsService)
    {
        _leagueService = leagueService;
        _applicationDbContextFactory = applicationDbContextFactory;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    public async IAsyncEnumerable<League> GetAllLeagues()
    {
        await _analyticsService.AddView(Request.GetRealIpAddress());
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        await foreach (var item in applicationDbContext.League.AsAsyncEnumerable().ConfigureAwait(false))
            yield return item;
    }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague()
    {
        _analyticsService.AddView(Request.GetRealIpAddress());
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        return _leagueService.GetCurrentLeague(applicationDbContext.League);
    }
}