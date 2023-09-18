using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Domain.Entity.Stats;

namespace PoEGamblingHelper.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public readonly DbSet<AnalyticsDay> AnalyticsDay = null!;
    public readonly DbSet<Currency> Currency = null!;
    public readonly DbSet<GemData> GemData = null!;
    public readonly DbSet<GemTradeData> GemTradeData = null!;
    public readonly DbSet<League> League = null!;
    public readonly DbSet<Result> Result = null!;
    public readonly DbSet<TempleCost> TempleCost = null!;
    public readonly DbSet<View> View = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public void ClearTrackedEntities() { ChangeTracker.Clear(); }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}