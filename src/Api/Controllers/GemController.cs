using Application.Services;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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