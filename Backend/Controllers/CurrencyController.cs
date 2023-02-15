using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Shared.Entity;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly IRepository<Currency, string> _currencyRepository;

    public CurrencyController(IRepository<Currency, string> currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() { return _currencyRepository.GetAllAsync(); }
}