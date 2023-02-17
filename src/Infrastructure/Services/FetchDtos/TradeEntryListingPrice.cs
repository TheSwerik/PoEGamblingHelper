using Domain;
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
        var currency = currencySet.FirstOrDefault(c => c.Name.EqualsIgnoreCase(Currency + " orb"));
        var conversionValue = currency?.ChaosEquivalent ?? 0;
        return Amount * conversionValue;
    }
}