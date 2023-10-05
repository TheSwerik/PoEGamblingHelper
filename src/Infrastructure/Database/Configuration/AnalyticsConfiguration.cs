using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Infrastructure.Database.Configuration;

public class AnalyticsDayConfiguration : IEntityTypeConfiguration<AnalyticsDay>
{
    public void Configure(EntityTypeBuilder<AnalyticsDay> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Date)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(a => a.Views)
               .IsRequired();
    }
}

public class ViewConfiguration : IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(v => v.TimeStamp)
               .IsRequired()
               .ValueGeneratedOnAdd();
    }
}