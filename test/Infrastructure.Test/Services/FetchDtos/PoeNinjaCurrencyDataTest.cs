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

        currency.Id.Should().Be(source.DetailsId);
        currency.Icon.Should().Be(source.Icon);
        currency.Name.Should().Be(source.Name);
        currency.ChaosEquivalent.Should().Be(source.ChaosEquivalent);
    }
}