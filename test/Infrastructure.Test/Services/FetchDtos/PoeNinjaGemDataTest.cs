using Infrastructure.Services.FetchDtos;

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