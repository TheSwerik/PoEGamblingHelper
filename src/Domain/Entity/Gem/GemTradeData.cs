using Domain.Entity.Abstract;

namespace Domain.Entity.Gem;

public partial class GemTradeData : Entity<long>
{
    public string Name { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }
    public string DetailsId { get; set; } = null!;
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }
}