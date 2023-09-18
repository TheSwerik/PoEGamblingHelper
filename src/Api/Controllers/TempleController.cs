using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Api.Extensions;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class TempleController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ITempleRepository _templeRepository;

    public TempleController(IAnalyticsService analyticsService, ITempleRepository templeRepository)
    {
        _analyticsService = analyticsService;
        _templeRepository = templeRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public TempleCost Get()
    {
        _analyticsService.AddView(Request.GetRealIpAddress());
        return _templeRepository.GetCurrent();
    }
}