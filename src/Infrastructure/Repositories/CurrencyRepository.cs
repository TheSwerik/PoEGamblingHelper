using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class CurrencyRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : ICurrencyRepository
{
    public async IAsyncEnumerable<Currency> GetAll(string league)
    {
        await using var applicationDbContext = await dbContextFactory.CreateDbContextAsync();
        await foreach (var item in applicationDbContext.Currency.Where(c => c.League.Equals(league)).AsAsyncEnumerable())
            yield return item;
    }
}