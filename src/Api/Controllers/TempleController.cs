using Application.Services;
using Domain.Entity;
using Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class TempleController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;

    public TempleController(IApplicationDbContextFactory applicationDbContextFactory,
                            IAnalyticsService analyticsService)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public TempleCost Get()
    {
        _analyticsService.AddView(Request.GetRealIpAddress());
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        return applicationDbContext.TempleCost.OrderByDescending(cost => cost.TimeStamp).FirstOrDefault()
               ?? throw new NoTempleDataException();
    }
}