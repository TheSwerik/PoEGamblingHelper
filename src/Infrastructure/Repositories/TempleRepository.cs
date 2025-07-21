using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public class TempleRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : ITempleRepository
{
    public TempleCost GetCurrent(string league)
    {
        using var applicationDbContext = dbContextFactory.CreateDbContext();
        return applicationDbContext.TempleCost.Where(c => c.League.Equals(league)).OrderByDescending(cost => cost.TimeStamp).FirstOrDefault() ??
               throw new NoTempleDataException();
    }
}