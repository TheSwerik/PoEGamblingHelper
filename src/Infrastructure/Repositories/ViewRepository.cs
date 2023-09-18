using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class ViewRepository : IViewRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ViewRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(View view)
    {
        view.TimeStamp = new DateTime(view.TimeStamp.Ticks, DateTimeKind.Utc);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        if (await context.View.AnyAsync(v => v.IpHash.Equals(view.IpHash) && v.TimeStamp == view.TimeStamp)) return;

        context.View.Add(view);
        await context.SaveChangesAsync();
    }

    public async Task<int> CountViewsAsync(DateOnly date)
    {
        var timeStamp = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.View.CountAsync(v => v.TimeStamp == timeStamp);
    }

    public async Task RemoveAllAsync(DateOnly date)
    {
        var timeStamp = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var views = context.View.Where(v => v.TimeStamp <= timeStamp);
        context.View.RemoveRange(views);

        await context.SaveChangesAsync();
    }
}