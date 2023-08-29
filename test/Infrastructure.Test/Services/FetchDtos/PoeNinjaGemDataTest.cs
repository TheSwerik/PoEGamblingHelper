using FluentAssertions;
using PoEGamblingHelper.Infrastructure.Services.FetchDtos;

namespace Infrastructure.Test.Services.FetchDtos;

public class PoeNinjaGemDataTest
{
    [Fact]
    public void ToGemTradeDataTest()
    {
        var source = new PoeNinjaGemData
                     {
                         Id = 123456,
                         Name = "Empower Support",
                         Icon = "testIcon",
                         GemLevel = 3,
                         GemQuality = 17,
                         Corrupted = true,
                         DetailsId = "empower-support",
                         ChaosValue = 100,
                         ExaltedValue = 8,
                         DivineValue = 0.5m,
                         ListingCount = 7
                     };

        var gemTradeData = source.ToGemTradeData();

        gemTradeData.Id.Should().Be(source.Id);
        gemTradeData.Name.Should().Be(source.Name);
        gemTradeData.GemLevel.Should().Be(source.GemLevel);
        gemTradeData.GemQuality.Should().Be(source.GemQuality);
        gemTradeData.Corrupted.Should().Be(source.Corrupted);
        gemTradeData.DetailsId.Should().Be(source.DetailsId);
        gemTradeData.ChaosValue.Should().Be(source.ChaosValue);
        gemTradeData.ExaltedValue.Should().Be(source.ExaltedValue);
        gemTradeData.DivineValue.Should().Be(source.DivineValue);
        gemTradeData.ListingCount.Should().Be(source.ListingCount);
    }
}