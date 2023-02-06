namespace Model.QueryParameters;

public class GemDataQuery
{
    public string SearchText { get; set; } = string.Empty;
    public Sort Sort { get; set; } = Sort.AverageProfitPerTryDesc;
    public GemType GemType { get; set; } = GemType.All;
    public bool OnlyShowProfitable { get; set; } = false;
    public bool ShowAlternateQuality { get; set; } = false;
    public decimal? PricePerTryFrom { get; set; } = null;
    public decimal? PricePerTryTo { get; set; } = null;

    public string ToQueryString(bool questionMark = true)
    {
        var start = questionMark ? "?" : "&";
        var searchText = SearchText == string.Empty ? "" : $"&searchText={SearchText}";
        var pricePerTryFrom = PricePerTryFrom is null ? "" : $"&pricePerTryFrom={PricePerTryFrom}";
        var pricePerTryTo = PricePerTryTo is null ? "" : $"&pricePerTryTo={PricePerTryTo}";
        return
            $"{start}sort={Sort}&gemType={GemType}&showAlternateQuality={ShowAlternateQuality}&onlyShowProfitable={OnlyShowProfitable}{searchText}{pricePerTryFrom}{pricePerTryTo}";
    }

    public string ToQueryString(PageRequest? page)
    {
        return page is null
                   ? ToQueryString()
                   : $"{page.ToQueryString()}{ToQueryString(false)}";
    }
}