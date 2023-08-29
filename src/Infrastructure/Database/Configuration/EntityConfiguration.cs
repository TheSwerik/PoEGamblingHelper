using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class EntityGuidConfiguration : IEntityTypeConfiguration<Entity<Guid>>
{
    public void Configure(EntityTypeBuilder<Entity<Guid>> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
    }
}

public class EntityLongConfiguration : IEntityTypeConfiguration<Entity<long>>
{
    public void Configure(EntityTypeBuilder<Entity<long>> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
    }
}

public class EntityStringConfiguration : IEntityTypeConfiguration<Entity<string>>
{
    public void Configure(EntityTypeBuilder<Entity<string>> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
    }
}