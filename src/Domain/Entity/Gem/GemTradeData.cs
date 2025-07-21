using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity.Gem;

public class GemTradeData : Entity<long>
{
    public string Name { get; set; } = null!;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }
    public string DetailsId { get; set; } = null!;
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }
    public string League { get; set; } = null!;
}