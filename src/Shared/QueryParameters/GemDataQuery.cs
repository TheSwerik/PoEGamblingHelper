namespace PoEGamblingHelper.Shared.QueryParameters
{
    public class GemDataQuery
    {
        public string SearchText { get; set; } = string.Empty;
        public Sort Sort { get; init; } = Sort.AverageProfitPerTryDesc;
        public GemType GemType { get; init; } = GemType.All;
        public bool OnlyShowProfitable { get; init; }
        public bool ShowAlternateQuality { get; init; }
        public bool ShowVaal { get; set; } = false;
        public decimal? PricePerTryFrom { get; set; }
        public decimal? PricePerTryTo { get; set; }
    }
}