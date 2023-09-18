using System;
using System.Linq;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity;

public class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public decimal[] ChaosValue { get; set; } = Array.Empty<decimal>();
    public decimal AverageChaosValue() { return ChaosValue.Average(); }
}