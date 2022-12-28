namespace Model;

public class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public decimal[] ChaosValue { get; set; }
    public decimal AverageChaosValue() { return ChaosValue.Average(); }
}