using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Services;
using Domain.Entity;
using Domain.Entity.Gem;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<Currency> Currency { get; init; } = null!;
    public virtual DbSet<League> League { get; init; } = null!;
    public virtual DbSet<TempleCost> TempleCost { get; init; } = null!;
    public virtual DbSet<GemData> GemData { get; init; } = null!;

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