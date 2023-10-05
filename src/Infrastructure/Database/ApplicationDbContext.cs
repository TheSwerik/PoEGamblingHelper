using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public readonly DbSet<AnalyticsDay> AnalyticsDay = null!;
    public readonly DbSet<Currency> Currency = null!;
    public readonly DbSet<GemData> GemData = null!;
    public readonly DbSet<GemTradeData> GemTradeData = null!;
    public readonly DbSet<League> League = null!;
    public readonly DbSet<TempleCost> TempleCost = null!;
    public readonly DbSet<View> View = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}