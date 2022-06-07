namespace PoEGamblingHelper2;

public record Item
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Variant { get; set; }
    public bool Corrupted { get; set; }
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public long Count { get; set; }
    public long ListingCount { get; set; }
}