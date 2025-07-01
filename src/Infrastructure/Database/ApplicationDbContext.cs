using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Domain.Entity.Stats;

namespace PoEGamblingHelper.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AnalyticsDay> AnalyticsDay { get; set; } = null!;
    public DbSet<Currency> Currency { get; set; } = null!;
    public DbSet<GemData> GemData { get; set; } = null!;
    public DbSet<GemTradeData> GemTradeData { get; set; } = null!;
    public DbSet<League> League { get; set; } = null!;
    public DbSet<TempleCost> TempleCost { get; set; } = null!;
    public DbSet<View> View { get; set; } = null!;
    public DbSet<Result> Result { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}