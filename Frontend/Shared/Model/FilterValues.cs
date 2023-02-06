using Model;
using Model.QueryParameters;

namespace PoEGamblingHelper3.Shared.Model;

public class FilterValues
{
    public string Gem { get; set; } = string.Empty;
    public Sort Sort { get; set; } = Sort.AverageProfitPerTryDesc;
    public GemType GemType { get; set; } = GemType.All;
    public decimal? PricePerTryFrom { get; set; } = null;
    public decimal? PricePerTryTo { get; set; } = null;
    public bool OnlyShowProfitable { get; set; } = false;
    public bool ShowAlternateQuality { get; set; } = false;
    public decimal? CurrencyValue { get; set; } = null;
    public decimal? TempleCost { get; set; } = null;
    public Currency? Currency { get; set; } = null;

    public GemDataQuery ToQuery()
    {
        return new GemDataQuery
               {
                   SearchText = Gem,
                   Sort = Sort,
                   GemType = GemType,
                   ShowAlternateQuality = ShowAlternateQuality,
                   OnlyShowProfitable = OnlyShowProfitable,
                   PricePerTryFrom = PricePerTryFrom,
                   PricePerTryTo = PricePerTryTo
               };
    }
}