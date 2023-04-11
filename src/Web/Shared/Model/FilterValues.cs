using Domain.Entity;
using Domain.QueryParameters;

namespace Web.Shared.Model;

public class FilterValues
{
    public string Gem { get; set; } = string.Empty;
    public Sort Sort { get; set; } = Sort.AverageProfitPerTryDesc;
    public GemType GemType { get; set; } = GemType.All;
    public decimal? PricePerTryFrom { get; set; }
    public decimal? PricePerTryTo { get; set; }
    public bool OnlyShowProfitable { get; set; }
    public bool ShowAlternateQuality { get; set; }
    public decimal? CurrencyValue { get; set; }
    public decimal? TempleCost { get; set; }
    public Currency? Currency { get; set; }

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