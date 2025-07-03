using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class AnalyticsDayRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IAnalyticsDayRepository
{
    public async Task AddAsync(int viewCount, DateOnly date)
    {
        var analyticsDay = new AnalyticsDay
        {
            Views = viewCount,
            Date = date
        };
        await using var context = await dbContextFactory.CreateDbContextAsync();
        context.AnalyticsDay.Add(analyticsDay);
        await context.SaveChangesAsync();
    }

    public async IAsyncEnumerable<AnalyticsDay> GetAll()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var items = context.AnalyticsDay
                           .OrderByDescending(day => day.Date)
                           .AsAsyncEnumerable()
                           .ConfigureAwait(false);
        await foreach (var item in items) yield return item;
    }

    public async IAsyncEnumerable<AnalyticsDay> Get(DateOnly startDate, DateOnly endDate)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var items = context.AnalyticsDay
                           .Where(a => a.Date >= startDate && a.Date <= endDate)
                           .OrderByDescending(day => day.Date)
                           .AsAsyncEnumerable()
                           .ConfigureAwait(false);
        await foreach (var item in items) yield return item;
    }
}