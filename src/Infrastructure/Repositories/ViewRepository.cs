using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class ViewRepository : IViewRepository
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IHashingService _hashingService;

    public ViewRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory,
                          IDateTimeService dateTimeService,
                          IHashingService hashingService)
    {
        _dbContextFactory = dbContextFactory;
        _dateTimeService = dateTimeService;
        _hashingService = hashingService;
    }

    public async Task AddAsync(string ipAddress)
    {
        var view = new View
        {
            IpHash = _hashingService.HashIpAddress(ipAddress),
            TimeStamp = _dateTimeService.UtcToday()
        };

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        if (await context.View.AnyAsync(v => v.IpHash.Equals(view.IpHash) && v.TimeStamp == view.TimeStamp)) return;

        context.View.Add(view);
        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(DateOnly date)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        return await context.View.CountAsync(v => v.TimeStamp == date);
    }

    public async Task RemoveAllAsync(DateOnly date)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var views = context.View.Where(v => v.TimeStamp <= date);
        context.View.RemoveRange(views);

        await context.SaveChangesAsync();
    }
}