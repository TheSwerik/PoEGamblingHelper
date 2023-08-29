using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity
{
    public class Currency : Entity<string>
    {
        public string Name { get; set; } = string.Empty;
        public decimal ChaosEquivalent { get; set; }
        public string? Icon { get; set; }
    }
}