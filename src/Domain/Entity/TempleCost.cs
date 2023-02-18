using Domain.Entity.Abstract;

namespace Domain.Entity;

public partial class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.Now.ToUniversalTime();
    public decimal[] ChaosValue { get; set; } = Array.Empty<decimal>();
    public decimal AverageChaosValue() { return ChaosValue.Average(); }
}