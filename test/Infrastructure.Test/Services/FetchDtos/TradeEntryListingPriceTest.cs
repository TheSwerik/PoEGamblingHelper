using MockQueryable.Moq;
using PoEGamblingHelper.Domain.Entity;

namespace Infrastructure.Test.Services.FetchDtos;

public class TradeEntryListingPriceTest
{
    private const decimal DivineChaosEquivalent = 220;
    private const decimal ExaltedChaosEquivalent = 12;
    private const decimal test = 0.6m;

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

        var source = new TradeEntryListingPrice
                     {
                         Type = "Chronicle of Atzoatl",
                         Amount = currencyAmount,
                         Currency = currency
                     };


        source.ChaosAmount(queryable.Object).Should().Be(currencyChaosEquivalent * currencyAmount);
    }
}