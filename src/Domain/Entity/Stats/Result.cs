using Domain.Entity.Gem;

namespace Domain.Entity.Stats;

public class Result: Abstract.Entity<Guid>
{
    
    public GemTradeData GemTradeData { get; set; } 
    public decimal CurrencyValue { get; set; } 
    public CurrencyResult CurrencyResult { get; set; } 
}