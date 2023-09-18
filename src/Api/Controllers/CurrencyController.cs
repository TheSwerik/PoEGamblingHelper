using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure;

namespace PoEGamblingHelper.Api.Controllers;

public class CurrencyController(ICurrencyRepository currencyRepository) : ApiControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = Constants.DataFetcherCacheTag)]
    public IAsyncEnumerable<Currency> GetAll() { return currencyRepository.GetAll(); }
}