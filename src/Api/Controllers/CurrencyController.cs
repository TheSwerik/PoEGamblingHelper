using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;

    public CurrencyController(IApplicationDbContextFactory applicationDbContextFactory,
                              IAnalyticsService analyticsService)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public async IAsyncEnumerable<Currency> GetAll()
    {
        await _analyticsService.AddView(Request.HttpContext.Connection.RemoteIpAddress);
        await _analyticsService.LogYesterdaysViews();
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        await foreach (var item in applicationDbContext.Currency.AsAsyncEnumerable().ConfigureAwait(false))
            yield return item;
    }
}