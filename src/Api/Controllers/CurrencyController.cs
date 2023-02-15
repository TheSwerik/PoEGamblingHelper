using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService) { _currencyService = currencyService; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() { return _currencyService.GetAllAsync(); }
}