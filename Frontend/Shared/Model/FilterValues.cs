namespace PoEGamblingHelper3.Shared.Model;

public class FilterValues
{
    public Gem? Gem { get; set; }
    public Sort? Sort { get; set; }
    public List<GemType> GemTypes { get; } = new();
    public int PricePerTryFrom { get; set; }
    public int PricePerTryTo { get; set; }
    public bool OnlyShowProfitable { get; set; }
}