using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Stats;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class ResultRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IResultRepository
{
    public async IAsyncEnumerable<Result> GetAll()
    {
        await using var applicationDbContext = dbContextFactory.CreateDbContext();
        await foreach (var item in applicationDbContext.Result
                                                       .Include(r => r.CurrencyResult)
                                                       .Include(r => r.GemTradeData)
                                                       .AsAsyncEnumerable()
                                                       .ConfigureAwait(false))
            yield return item;
    }

    public async Task<Result> SaveAsync(Result result)
    {
        await using var applicationDbContext = dbContextFactory.CreateDbContext();
        var response = await applicationDbContext.Result.AddAsync(result);
        await applicationDbContext.SaveChangesAsync();
        return response.Entity;
    }
}