namespace Backend.Model;

public class GemData : Gem
{
    public string DetailsId { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }
    public bool Corrupted { get; set; }
    public int GemQuality { get; set; }
    public override string ToString() { return Name; }
}