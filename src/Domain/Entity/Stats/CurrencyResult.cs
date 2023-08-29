using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity.Stats
{
    public class CurrencyResult : Entity<string>
    {
        public string Name { get; set; } = string.Empty;
        public decimal ChaosEquivalent { get; set; }
    }
}