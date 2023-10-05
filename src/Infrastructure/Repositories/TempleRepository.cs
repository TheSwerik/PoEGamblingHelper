using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class TempleRepository : ITempleRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public TempleRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public TempleCost GetCurrent()
    {
        using var applicationDbContext = _dbContextFactory.CreateDbContext();
        return applicationDbContext.TempleCost.OrderByDescending(cost => cost.TimeStamp).FirstOrDefault()
               ?? throw new NoTempleDataException();
    }
}