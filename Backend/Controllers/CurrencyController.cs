using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Model;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly IRepository<Currency, string> _currencyRepository;
    private readonly ILogger<CurrencyController> _logger;

    public CurrencyController(ILogger<CurrencyController> logger, IRepository<Currency, string> currencyRepository)
    {
        _logger = logger;
        _currencyRepository = currencyRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public ActionResult<IAsyncEnumerable<GemData>> GetAll() { return Ok(_currencyRepository.GetAllAsync()); }
}