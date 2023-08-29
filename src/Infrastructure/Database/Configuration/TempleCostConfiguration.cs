using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class TempleCostConfiguration : IEntityTypeConfiguration<TempleCost>
{
    public void Configure(EntityTypeBuilder<TempleCost> builder)
    {
        var options = new JsonSerializerOptions();
        builder.Property(p => p.ChaosValue)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, options),
                   v => JsonSerializer.Deserialize<decimal[]>(v, options)!);
    }
}