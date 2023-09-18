using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class LeagueRepository : ILeagueRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public LeagueRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public League GetByStartDateAfter(DateTime dateTime)
    {
        using var applicationDbContext = _dbContextFactory.CreateDbContext();
        return applicationDbContext.League.Where(league => dateTime >= league.StartDate)
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