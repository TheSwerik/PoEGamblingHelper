using FluentAssertions;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.Test.DataFetcher.FetchDtos;

public class PoeNinjaGemDataTest
{
    [Fact]
    public void ToGemTradeDataTest()
    {
        var source = new PoeNinjaGemData(
            123456,
            "Empower Support",
            "testIcon",
            3,
            17,
            true,
            "empower-support",
            100,
            8,
            0.5m,
            7
        );

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