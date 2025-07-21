using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.Test.DataFetcher.FetchDtos;

public class PoeNinjaCurrencyDataTest
{
    [Fact]
    public void ToCurrencyDataTest()
    {
        var source = new PoeNinjaCurrencyData
        {
            CurrencyTypeName = "Divine Orb",
            Icon = "testIcon",
            ChaosEquivalent = 200,
            DetailsId = "divine-orb"
        };
        var league = "Standard";

        var currency = source.ToCurrencyData(league);

        currency.Id.ShouldBe(source.DetailsId);
        currency.Icon.ShouldBe(source.Icon);
        currency.Name.ShouldBe(source.CurrencyTypeName);
        currency.ChaosEquivalent.ShouldBe(source.ChaosEquivalent);
        currency.League.ShouldBe(league);
    }
}