namespace Domain.Entity.Stats;

public class CurrencyResult: Abstract.Entity<string>
{
    public string Name { get; set; } = string.Empty;
    public decimal ChaosEquivalent { get; set; }
}