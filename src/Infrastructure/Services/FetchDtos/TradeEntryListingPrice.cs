using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.FetchDtos;

public class TradeEntryListingPrice
{
    public string Type { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;

    public decimal ChaosAmount(DbSet<Currency> currencySet)
    {
        var lowerCurrencyName = (Currency + " orb").ToLower();
        var currency = currencySet.FirstOrDefault(c => c.Name.ToLower().Equals(lowerCurrencyName));
        var conversionValue = currency?.ChaosEquivalent ?? 1;
        return Amount * conversionValue;
    }
}