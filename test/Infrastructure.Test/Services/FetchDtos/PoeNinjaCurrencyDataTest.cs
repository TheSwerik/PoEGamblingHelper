using Infrastructure.Services.FetchDtos;

namespace Infrastructure.Test.Services.FetchDtos;

public class PoeNinjaCurrencyDataTest
{
    [Fact]
    public void ToCurrencyDataTest()
    {
        var source = new PoeNinjaCurrencyData
        {
            Name = "Divine Orb",
            Icon = "testIcon",
            ChaosEquivalent = 200,
            DetailsId = "divine-orb"
        };

        var currency = source.ToCurrencyData();

        currency.Id.ShouldBe(source.DetailsId);
        currency.Icon.ShouldBe(source.Icon);
        currency.Name.ShouldBe(source.Name);
        currency.ChaosEquivalent.ShouldBe(source.ChaosEquivalent);
    }
}