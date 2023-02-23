using Domain.Entity.Gem;

namespace Infrastructure.Services.FetchDtos;

public class PoeNinjaGemData
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }
    public string DetailsId { get; set; } = null!;
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }

    public GemTradeData ToGemTradeData()
    {
        return new GemTradeData
               {
                   Id = Id,
                   Name = Name,
                   GemLevel = GemLevel,
                   GemQuality = GemQuality,
                   Corrupted = Corrupted,
                   DetailsId = DetailsId,
                   ChaosValue = ChaosValue,
                   ExaltedValue = ExaltedValue,
                   DivineValue = DivineValue,
                   ListingCount = ListingCount
               };
    }
}