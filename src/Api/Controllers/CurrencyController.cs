using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Repositories;

namespace PoEGamblingHelper.Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly CurrencyRepository _currencyRepository;

    public CurrencyController(CurrencyRepository currencyRepository) { _currencyRepository = currencyRepository; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() //TODO
    {
        return _currencyRepository.GetAll();
    }
}