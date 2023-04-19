using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Services;
using Domain.Entity;
using Domain.Entity.Analytics;
using Domain.Entity.Gem;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<Currency> Currency => Set<Currency>();
    public virtual DbSet<League> League => Set<League>();
    public virtual DbSet<TempleCost> TempleCost => Set<TempleCost>();
    public virtual DbSet<GemData> GemData => Set<GemData>();
    public virtual DbSet<GemTradeData> GemTradeData => Set<GemTradeData>();
    public virtual DbSet<View> View => Set<View>();
    public Task<int> SaveChangesAsync() { return base.SaveChangesAsync(); }

    public void ClearTrackedEntities() { ChangeTracker.Clear(); }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var options = new JsonSerializerOptions();
        modelBuilder.Entity<TempleCost>().Property(p => p.ChaosValue)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, options),
                        v => JsonSerializer.Deserialize<decimal[]>(v, options)!);
    }
}