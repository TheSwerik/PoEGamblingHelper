namespace Domain.QueryParameters;

public class GemDataQuery
{
    public string SearchText { get; set; } = string.Empty;
    public Sort Sort { get; init; } = Sort.AverageProfitPerTryDesc;
    public GemType GemType { get; init; } = GemType.All;
    public bool OnlyShowProfitable { get; init; } = false;
    public bool ShowAlternateQuality { get; init; } = false;
    public bool ShowVaal { get; set; } = false;
    public decimal? PricePerTryFrom { get; set; }
    public decimal? PricePerTryTo { get; set; }

    //TODO move to frontend
    public string ToQueryString(bool questionMark = true)
    {
        var start = questionMark ? "?" : "&";
        var searchText = SearchText == string.Empty ? "" : $"&searchText={SearchText}";
        var pricePerTryFrom = PricePerTryFrom is null ? "" : $"&pricePerTryFrom={PricePerTryFrom}";
        var pricePerTryTo = PricePerTryTo is null ? "" : $"&pricePerTryTo={PricePerTryTo}";
        return
            $"{start}sort={Sort}&gemType={GemType}&showAlternateQuality={ShowAlternateQuality}&onlyShowProfitable={OnlyShowProfitable}&showVaal={ShowVaal}{searchText}{pricePerTryFrom}{pricePerTryTo}";
    }

    //TODO move to frontend
    public string ToQueryString(PageRequest? page)
    {
        return page is null
                   ? ToQueryString()
                   : $"{page.ToQueryString()}{ToQueryString(false)}";
    }
}