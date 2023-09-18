using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Domain.Entity.Stats;

namespace PoEGamblingHelper.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<Currency> Currency => Set<Currency>();
    public virtual DbSet<League> League => Set<League>();
    public virtual DbSet<TempleCost> TempleCost => Set<TempleCost>();
    public virtual DbSet<GemData> GemData => Set<GemData>();
    public virtual DbSet<GemTradeData> GemTradeData => Set<GemTradeData>();
    public virtual DbSet<Result> Result => Set<Result>();
    public virtual DbSet<View> View => Set<View>();
    public Task<int> SaveChangesAsync() { return base.SaveChangesAsync(); }

    public void ClearTrackedEntities() { ChangeTracker.Clear(); }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}