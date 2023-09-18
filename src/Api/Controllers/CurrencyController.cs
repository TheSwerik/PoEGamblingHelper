using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyController(ICurrencyRepository currencyRepository) { _currencyRepository = currencyRepository; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() { return _currencyRepository.GetAll(); }
}