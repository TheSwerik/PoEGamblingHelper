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

        var currency = source.ToCurrencyData();

        currency.Id.ShouldBe(source.DetailsId);
        currency.Icon.ShouldBe(source.Icon);
        currency.Name.ShouldBe(source.CurrencyTypeName);
        currency.ChaosEquivalent.ShouldBe(source.ChaosEquivalent);
    }
}