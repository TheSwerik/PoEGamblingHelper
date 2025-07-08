using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity;

public class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public decimal[] ChaosValue { get; set; } = [];
    public string League { get; set; } = null!;

    public decimal AverageChaosValue()
    {
        return ChaosValue.Average();
    }
}