using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class TempleCostConfiguration : IEntityTypeConfiguration<TempleCost>
{
    public void Configure(EntityTypeBuilder<TempleCost> builder)
    {
        builder.HasKey(t => new { t.Id, t.League });
        builder.Property(t => t.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(t => t.League)
               .IsRequired();

        builder.Property(t => t.TimeStamp)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(t => t.ChaosValue)
               .IsRequired();
    }
}