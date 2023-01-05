using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<GemData> GemData { get; init; } = null!;
    public virtual DbSet<GemTradeData> GemTradeData { get; init; } = null!;
    public virtual DbSet<Currency> Currency { get; init; } = null!;
    public virtual DbSet<League> Leagues { get; init; } = null!;
    public virtual DbSet<TempleCost> TempleCost { get; init; } = null!;

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