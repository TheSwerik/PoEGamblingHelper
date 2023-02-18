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
    public async IAsyncEnumerable<Currency> GetAll()
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        await foreach (var item in applicationDbContext.Currency.AsAsyncEnumerable().ConfigureAwait(false))
            yield return item;
    }
}