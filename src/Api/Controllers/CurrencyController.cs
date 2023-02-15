using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CurrencyController(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll() { return _applicationDbContext.Currency.AsAsyncEnumerable(); }
}