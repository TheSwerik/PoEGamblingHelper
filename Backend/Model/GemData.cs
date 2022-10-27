namespace Backend.Model;

public class GemData : Gem
{
    private static readonly List<string> AlternateQualities = new() { "Anomalous", "Divergent", "Phantasmal" };
    public string DetailsId { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }
    public bool Corrupted { get; set; }
    public int GemQuality { get; set; }

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

    public string TradeUrl()
    {
        var name = Name;
        var gemAlternateQuality = 0;

        Console.WriteLine(name);
        var firstWord = name.Split(" ")[0];
        if (AlternateQualities.Contains(firstWord))
        {
            name = name[(firstWord.Length + 1)..];
            Console.WriteLine(name);
            gemAlternateQuality = AlternateQualities.IndexOf(firstWord) + 1;
        }

        return
            $"{{\"query\":{{\"filters\":{{\"misc_filters\":{{\"filters\":{{\"gem_level\":{{\"min\":{GemLevel},\"max\":{GemLevel}}},\"gem_alternate_quality\":{{\"option\":\"{gemAlternateQuality}\"}},\"quality\":{{\"min\":{GemQuality},\"max\":{GemQuality}}}}}}}}},\"type\":\"{name}\"}}}}";
    }
}