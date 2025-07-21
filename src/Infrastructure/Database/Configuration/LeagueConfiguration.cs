using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(l => l.Name)
               .IsRequired();

        builder.Property(l => l.StartDate)
               .IsRequired();

        builder.Property(l => l.Version)
               .IsRequired();
    }
}