using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class LeagueRepository : ILeagueRepository
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public LeagueRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, IDateTimeService dateTimeService)
    {
        _dbContextFactory = dbContextFactory;
        _dateTimeService = dateTimeService;
    }

    public League GetCurrent() { return GetByStartDateBefore(_dateTimeService.UtcNow()); }

    public League GetByStartDateBefore(DateTime dateTime)
    {
        using var applicationDbContext = _dbContextFactory.CreateDbContext();
        return applicationDbContext.League.Where(league => league.StartDate <= dateTime)
                                   .OrderByDescending(league => league.StartDate)
                                   .FirstOrDefault()
               ?? throw new NoLeagueDataException();
    }

    public async IAsyncEnumerable<League> GetAllLeagues()
    {
        await using var applicationDbContext = await _dbContextFactory.CreateDbContextAsync();
        await foreach (var item in applicationDbContext.League.AsAsyncEnumerable().ConfigureAwait(false))
            yield return item;
    }
}