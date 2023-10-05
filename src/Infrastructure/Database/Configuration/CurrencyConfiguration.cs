using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .IsRequired();

        builder.Property(c => c.ChaosEquivalent)
               .IsRequired();
    }
}