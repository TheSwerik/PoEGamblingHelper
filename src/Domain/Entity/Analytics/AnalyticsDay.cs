using System;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity.Analytics;

public class AnalyticsDay : Entity<Guid>
{
    public DateOnly Date { get; set; }
    public long Views { get; set; }
}