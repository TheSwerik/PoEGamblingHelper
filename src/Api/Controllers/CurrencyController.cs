using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Api.Extensions;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Repositories;

namespace PoEGamblingHelper.Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly CurrencyRepository _currencyRepository;

    public CurrencyController(IAnalyticsService analyticsService,
                              CurrencyRepository currencyRepository)
    {
        _analyticsService = analyticsService;
        _currencyRepository = currencyRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() //TODO
    {
        _analyticsService.AddView(Request.GetRealIpAddress()).RunSynchronously();
        return _currencyRepository.GetAll();
    }
}