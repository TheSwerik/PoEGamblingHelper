using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public record CurrencyPriceData(PoeNinjaCurrencyData[] Lines, PoeNinjaCurrencyDetails[] CurrencyDetails);

public record GemPriceData(PoeNinjaGemData[] Lines);

public class PoeNinjaCurrencyData
{
    public string CurrencyTypeName { get; init; }
    public decimal ChaosEquivalent { get; init; }
    public string DetailsId { get; init; }
    public string? Icon { get; set; }

    public Currency ToCurrencyData()
    {
        return new Currency
               {
                   Id = DetailsId,
                   Name = CurrencyTypeName,
                   ChaosEquivalent = ChaosEquivalent,
                   Icon = Icon
               };
    }
}

public record PoeNinjaCurrencyDetails(string Name, string? Icon);

public record PoeNinjaGemData(long Id,
                              string Name,
                              string Icon,
                              int GemLevel,
                              int GemQuality,
                              bool Corrupted,
                              string DetailsId,
                              decimal ChaosValue,
                              decimal ExaltedValue,
                              decimal DivineValue,
                              int ListingCount)
{
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

public record TradeEntry(string Id, TradeEntryListing Listing);

public record TradeEntryListing(TradeEntryListingPrice Price);

public record TradeEntryListingPrice(string Type, decimal Amount, string Currency)
{
    public decimal ChaosAmount(IEnumerable<Currency> currencySet) //TODO
    {
        var lowerCurrencyName = (Currency + " orb").ToLower();
        var currency = currencySet.FirstOrDefault(c => c.Name.ToLower().Equals(lowerCurrencyName));
        var conversionValue = currency?.ChaosEquivalent ?? 1;
        return Amount * conversionValue;
    }
}

public record TradeEntryResult(TradeEntry[] Result);

public record TradeResults(string Id, int Complexity, string[] Result, int Total);