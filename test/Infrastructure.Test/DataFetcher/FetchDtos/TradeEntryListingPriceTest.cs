using FluentAssertions;
using MockQueryable.Moq;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.Test.DataFetcher.FetchDtos;

public class TradeEntryListingPriceTest
{
    [Theory]
    [InlineData("chaos", 120, 1)]
    [InlineData("divine", 0.6, 220)]
    [InlineData("exalted", 3, 12)]
    public void ChaosAmountTest(string currency, decimal currencyAmount, decimal currencyChaosEquivalent)
    {
        var list = new List<Currency>
                   {
                       new() { ChaosEquivalent = 1, Name = "Chaos Orb" },
                       new() { ChaosEquivalent = 220, Name = "Divine Orb" },
                       new() { ChaosEquivalent = 12, Name = "Exalted Orb" }
                   };
        var queryable = list.AsQueryable().BuildMockDbSet();

        var source = new TradeEntryListingPrice("Chronicle of Atzoatl", currencyAmount, currency);

        source.ChaosAmount(queryable.Object).Should().Be(currencyChaosEquivalent * currencyAmount);
    }
}