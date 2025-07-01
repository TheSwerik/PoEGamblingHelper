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

        gemTradeData.Id.ShouldBe(source.Id);
        gemTradeData.Name.ShouldBe(source.Name);
        gemTradeData.GemLevel.ShouldBe(source.GemLevel);
        gemTradeData.GemQuality.ShouldBe(source.GemQuality);
        gemTradeData.Corrupted.ShouldBe(source.Corrupted);
        gemTradeData.DetailsId.ShouldBe(source.DetailsId);
        gemTradeData.ChaosValue.ShouldBe(source.ChaosValue);
        gemTradeData.ExaltedValue.ShouldBe(source.ExaltedValue);
        gemTradeData.DivineValue.ShouldBe(source.DivineValue);
        gemTradeData.ListingCount.ShouldBe(source.ListingCount);
    }
}