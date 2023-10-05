using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class AnalyticsDayRepository : IAnalyticsDayRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public AnalyticsDayRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(int viewCount, DateOnly date)
    {
        var analyticsDay = new AnalyticsDay
                           {
                               Views = viewCount,
                               Date = date
                           };
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        context.AnalyticsDay.Add(analyticsDay);
        await context.SaveChangesAsync();
    }
}