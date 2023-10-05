using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public CurrencyRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async IAsyncEnumerable<Currency> GetAll()
    {
        await using var applicationDbContext = await _dbContextFactory.CreateDbContextAsync();
        await foreach (var item in applicationDbContext.Currency.AsAsyncEnumerable())
            yield return item;
    }
}