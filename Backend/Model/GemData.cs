namespace Backend.Model;

public class GemData : Gem
{
    public string DetailsId { get; set; }
    public string Icon { get; set; } = string.Empty;
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }

    public override string ToString()
    {
        Console.WriteLine(
            base.ToString()[..(base.ToString().Length - 1)] +
            $", DetailsId={DetailsId}, GemLevel={GemLevel}, ChaosValue={ChaosValue}, ExaltedValue={ExaltedValue}, DivineValue={DivineValue}, Listings={ListingCount}, Corrupted={Corrupted}, Quality={GemQuality}]"
        );
        return string.Concat(
            base.ToString().AsSpan(0, base.ToString().Length - 1),
            $", DetailsId={DetailsId}, GemLevel={GemLevel}, ChaosValue={ChaosValue}, ExaltedValue={ExaltedValue}, DivineValue={DivineValue}, Listings={ListingCount}, Corrupted={Corrupted}, Quality={GemQuality}]"
        );
    }
}