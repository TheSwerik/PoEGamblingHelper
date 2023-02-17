using Application.Services;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class CurrencyController : ApiControllerBase
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;

    public CurrencyController(IApplicationDbContextFactory applicationDbContextFactory)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public IAsyncEnumerable<Currency> GetAll()
    {
        using var context = _applicationDbContextFactory.CreateDbContext();
        return context.Currency.AsAsyncEnumerable();
    }
}