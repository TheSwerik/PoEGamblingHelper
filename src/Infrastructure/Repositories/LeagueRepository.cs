using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class LeagueRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, IDateTimeService dateTimeService)
    : ILeagueRepository
{
    public League GetCurrent()
    {
        return GetByStartDateBefore(dateTimeService.UtcNow());
    }

    public League GetByStartDateBefore(DateTime dateTime)
    {
        using var applicationDbContext = dbContextFactory.CreateDbContext();
        return applicationDbContext.League
                                   .Where(league => league.StartDate <= dateTime)
                                   .OrderByDescending(league => league.StartDate)
                                   .FirstOrDefault() ??
               throw new NoLeagueDataException();
    }

    public async IAsyncEnumerable<League> GetAll()
    {
        await using var applicationDbContext = await dbContextFactory.CreateDbContextAsync();
        await foreach (var item in applicationDbContext.League.AsAsyncEnumerable().ConfigureAwait(false))
            yield return item;
    }
}