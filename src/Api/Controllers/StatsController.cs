using Application.Services;
using Domain.Entity;
using Domain.Entity.Stats;
using Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class StatsController : ApiControllerBase
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;

    public StatsController(IApplicationDbContextFactory applicationDbContextFactory)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    [HttpGet]
    public async IAsyncEnumerable<Result> GetAll()
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        await foreach (var item in applicationDbContext.Result
                                                       .Include(r => r.CurrencyResult)
                                                       .Include(r => r.GemTradeData)
                                                       .AsAsyncEnumerable()
                                                       .ConfigureAwait(false))
            yield return item;
    }

    [HttpPost]
    public async Task Post(Result result)
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        await applicationDbContext.Result.AddAsync(result);
        await applicationDbContext.SaveChangesAsync();
    }
}