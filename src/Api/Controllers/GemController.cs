using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Api.Controllers;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace Api.Controllers;

public class GemController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IGemService _gemService;

    public GemController(IGemService gemService, IAnalyticsService analyticsService)
    {
        _gemService = gemService;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public async Task<Page<GemData>> GetAll([FromQuery] GemDataQuery? query, [FromQuery] PageRequest page)
    {
        await _analyticsService.AddView(Request.GetRealIpAddress());
        return await _gemService.GetAll(query, page);
    }
}