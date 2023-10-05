using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class GemDataConfiguration : IEntityTypeConfiguration<GemData>
{
    public void Configure(EntityTypeBuilder<GemData> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.HasMany(g => g.Gems)
               .WithOne();

        builder.Property(g => g.Name)
               .IsRequired();
    }
}

public class GemTradeDataConfiguration : IEntityTypeConfiguration<GemTradeData>
{
    public void Configure(EntityTypeBuilder<GemTradeData> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
               .IsRequired();

        builder.Property(g => g.GemLevel)
               .IsRequired();

        builder.Property(g => g.GemQuality)
               .IsRequired();

        builder.Property(g => g.Corrupted)
               .IsRequired();

        builder.Property(g => g.DetailsId)
               .IsRequired();

        builder.Property(g => g.ChaosValue)
               .IsRequired();

        builder.Property(g => g.ExaltedValue)
               .IsRequired();

        builder.Property(g => g.DivineValue)
               .IsRequired();

        builder.Property(g => g.ListingCount)
               .IsRequired();
    }
}