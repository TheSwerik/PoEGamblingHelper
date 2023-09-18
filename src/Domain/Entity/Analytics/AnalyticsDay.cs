using System;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity.Analytics;

public class AnalyticsDay : Entity<Guid>
{
    public DateOnly TimeStamp { get; set; }
    public long Views { get; set; }
}