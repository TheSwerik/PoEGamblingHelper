namespace PoEGamblingHelper.Infrastructure.Services.FetchDtos;

public class TradeEntry
{
    public string Id { get; set; } = null!;

    public TradeEntryListing Listing { get; set; } = null!;
}