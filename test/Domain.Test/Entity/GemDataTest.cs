using Domain.Entity.Gem;

namespace Domain.Test.Entity;

public class GemDataTest
{
    [Theory]
    [InlineData("Enhance Support", true)]
    [InlineData("Empower Support", true)]
    [InlineData("Enlighten Support", true)]
    [InlineData("Brutality Support", false)]
    [InlineData("Seismic Trap", false)]
    public void IsExceptionalTest(string name, bool expected)
    {
        var gemData = new GemData();

        gemData.Name = name;
        gemData.IsExceptional().ShouldBe(expected);

        gemData.Name = name.ToLowerInvariant();
        gemData.IsExceptional().ShouldBe(expected);

        gemData.Name = $"Awakened {name}";
        gemData.IsExceptional().ShouldBe(expected);

        gemData.Name = $"Awakened {name.ToLowerInvariant()}";
        gemData.IsExceptional().ShouldBe(expected);
    }

    [Theory]
    [InlineData("Enhance Support", 3, 4)]
    [InlineData("Empower Support", 3, 4)]
    [InlineData("Enlighten Support", 3, 4)]
    [InlineData("Brutality Support", 20, 5)]
    [InlineData("Seismic Trap", 20, 5)]
    public void MaxLevelTest(string name, int expected, int expectedAwakened)
    {
        var gemData = new GemData();

        gemData.Name = name;
        gemData.MaxLevel().ShouldBe(expected);

        gemData.Name = name.ToLowerInvariant();
        gemData.MaxLevel().ShouldBe(expected);

        gemData.Name = $"Awakened {name}";
        gemData.MaxLevel().ShouldBe(expectedAwakened);

        gemData.Name = $"Awakened {name.ToLowerInvariant()}";
        gemData.MaxLevel().ShouldBe(expectedAwakened);
    }

    [Theory]
    [InlineData("Enhance Support", 3, 6)]
    [InlineData("Empower Support", 3, 6)]
    [InlineData("Enlighten Support", 3, 6)]
    [InlineData("Brutality Support", 12, 8)]
    [InlineData("Seismic Trap", 12, 8)]
    public void RawCostTest(string name, int expected, int expectedAwakened)
    {
        var gemData = new GemData
        {
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 1, GemQuality = 15 },
                new() { Corrupted = true, GemLevel = 3, ChaosValue = 2 },
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 3 },
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 4 },
                new() { Corrupted = true, GemLevel = 4, ChaosValue = 5 },
                new() { Corrupted = false, GemLevel = 4, ChaosValue = 6 },
                new() { Corrupted = true, GemLevel = 5, ChaosValue = 7 },
                new() { Corrupted = false, GemLevel = 5, ChaosValue = 8 },
                new() { Corrupted = true, GemLevel = 6, ChaosValue = 9 },
                new() { Corrupted = false, GemLevel = 6, ChaosValue = 10 },
                new() { Corrupted = true, GemLevel = 20, ChaosValue = 11 },
                new() { Corrupted = false, GemLevel = 20, ChaosValue = 12 },
                new() { Corrupted = true, GemLevel = 21, ChaosValue = 13 },
                new() { Corrupted = false, GemLevel = 21, ChaosValue = 14 }
            }
        };

        gemData.Name = name;
        gemData.RawCost().ShouldBe(expected);

        gemData.Name = $"Awakened {name}";
        gemData.RawCost().ShouldBe(expectedAwakened);
    }

    [Theory]
    [InlineData("Enhance Support", 11, 22)]
    [InlineData("Empower Support", 11, 22)]
    [InlineData("Enlighten Support", 11, 22)]
    [InlineData("Brutality Support", 44, 33)]
    [InlineData("Seismic Trap", 44, 33)]
    public void CostPerTryTest(string name, int expected, int expectedAwakened)
    {
        var gemData = new GemData
        {
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 11 },
                new() { Corrupted = false, GemLevel = 4, ChaosValue = 22 },
                new() { Corrupted = false, GemLevel = 5, ChaosValue = 33 },
                new() { Corrupted = false, GemLevel = 20, ChaosValue = 44 }
            }
        };

        const int rawCost = 5;
        const int templeCost = 7;

        gemData.Name = name;
        gemData.CostPerTry().ShouldBe(expected);
        gemData.CostPerTry().ShouldBe(expected);
        gemData.CostPerTry(rawCost).ShouldBe(rawCost);
        gemData.CostPerTry(templeCost: templeCost).ShouldBe(expected + templeCost);
        gemData.CostPerTry(null, templeCost).ShouldBe(expected + templeCost);
        gemData.CostPerTry(rawCost, templeCost).ShouldBe(rawCost + templeCost);

        gemData.Name = $"Awakened {name}";
        gemData.CostPerTry().ShouldBe(expectedAwakened);
        gemData.CostPerTry().ShouldBe(expectedAwakened);
        gemData.CostPerTry(rawCost).ShouldBe(rawCost);
        gemData.CostPerTry(templeCost: templeCost).ShouldBe(expectedAwakened + templeCost);
        gemData.CostPerTry(null, templeCost).ShouldBe(expectedAwakened + templeCost);
        gemData.CostPerTry(rawCost, templeCost).ShouldBe(rawCost + templeCost);
    }

    [Fact]
    public void ResultValueTest()
    {
        var gemData = new GemData
        {
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = true, GemLevel = 1, ChaosValue = 1 },
                new() { Corrupted = true, GemLevel = 1, ChaosValue = 2 },
                new() { Corrupted = true, GemLevel = 1, ChaosValue = 3, GemQuality = 15 },
                new() { Corrupted = false, GemLevel = 1, ChaosValue = 4 }
            }
        };
        gemData.ResultValue(1).ShouldBe(1);
        gemData.ResultValue(2).ShouldBe(0);
    }

    [Theory]
    [InlineData("Enhance Support", 15, 35)]
    [InlineData("Empower Support", 15, 35)]
    [InlineData("Enlighten Support", 15, 35)]
    [InlineData("Brutality Support", 95, 55)]
    [InlineData("Seismic Trap", 95, 55)]
    public void ValueTest(string name, int expectedMiddle, int expectedMiddleAwakened)
    {
        var gemData = new GemData
        {
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = true, GemLevel = 2, ChaosValue = 5 },
                new() { Corrupted = true, GemLevel = 3, ChaosValue = 15 },
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 25 },
                new() { Corrupted = true, GemLevel = 4, ChaosValue = 35 },
                new() { Corrupted = false, GemLevel = 4, ChaosValue = 45 },
                new() { Corrupted = true, GemLevel = 5, ChaosValue = 55 },
                new() { Corrupted = false, GemLevel = 5, ChaosValue = 65 },
                new() { Corrupted = true, GemLevel = 6, ChaosValue = 75 },
                new() { Corrupted = true, GemLevel = 19, ChaosValue = 85 },
                new() { Corrupted = true, GemLevel = 20, ChaosValue = 95 },
                new() { Corrupted = false, GemLevel = 20, ChaosValue = 105 },
                new() { Corrupted = true, GemLevel = 21, ChaosValue = 115 }
            }
        };

        gemData.Name = name;
        gemData.Value(ResultCase.Worst).ShouldBe(expectedMiddle - 10);
        gemData.Value(ResultCase.Middle).ShouldBe(expectedMiddle);
        gemData.Value(ResultCase.Best).ShouldBe(expectedMiddle + 20);

        gemData.Name = $"Awakened {name}";
        gemData.Value(ResultCase.Worst).ShouldBe(expectedMiddleAwakened - 20);
        gemData.Value(ResultCase.Middle).ShouldBe(expectedMiddleAwakened);
        gemData.Value(ResultCase.Best).ShouldBe(expectedMiddleAwakened + 20);
    }

    [Theory]
    [InlineData("Enhance Support", 25, 15, 45, 35)]
    [InlineData("Empower Support", 25, 15, 45, 35)]
    [InlineData("Enlighten Support", 25, 15, 45, 35)]
    [InlineData("Brutality Support", 105, 95, 65, 55)]
    [InlineData("Seismic Trap", 105, 95, 65, 55)]
    public void ProfitTest(string name,
                           int rawValue,
                           int expectedMiddle,
                           int rawValueAwakened,
                           int expectedMiddleAwakened)
    {
        var gemData = new GemData
        {
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = true, GemLevel = 2, ChaosValue = 5 },
                new() { Corrupted = true, GemLevel = 3, ChaosValue = 15 },
                new() { Corrupted = false, GemLevel = 3, ChaosValue = 25 },
                new() { Corrupted = true, GemLevel = 4, ChaosValue = 35 },
                new() { Corrupted = false, GemLevel = 4, ChaosValue = 45 },
                new() { Corrupted = true, GemLevel = 5, ChaosValue = 55 },
                new() { Corrupted = false, GemLevel = 5, ChaosValue = 65 },
                new() { Corrupted = true, GemLevel = 6, ChaosValue = 75 },
                new() { Corrupted = true, GemLevel = 19, ChaosValue = 85 },
                new() { Corrupted = true, GemLevel = 20, ChaosValue = 95 },
                new() { Corrupted = false, GemLevel = 20, ChaosValue = 105 },
                new() { Corrupted = true, GemLevel = 21, ChaosValue = 115 }
            }
        };
        const int rawCost = 100;
        const int templeCost = 50;
        const int value = 200;

        gemData.Name = name;
        gemData.Profit(ResultCase.Worst).ShouldBe(expectedMiddle - 10 - rawValue);
        gemData.Profit(ResultCase.Worst, null, templeCost).ShouldBe(expectedMiddle - 10 - rawValue - templeCost);
        gemData.Profit(ResultCase.Worst, rawCost).ShouldBe(expectedMiddle - 10 - rawCost);
        gemData.Profit(ResultCase.Worst, rawCost, templeCost).ShouldBe(expectedMiddle - 10 - rawCost - templeCost);
        gemData.Profit(ResultCase.Middle).ShouldBe(expectedMiddle - rawValue);
        gemData.Profit(ResultCase.Middle, null, templeCost).ShouldBe(expectedMiddle - rawValue - templeCost);
        gemData.Profit(ResultCase.Middle, rawCost).ShouldBe(expectedMiddle - rawCost);
        gemData.Profit(ResultCase.Middle, rawCost, templeCost).ShouldBe(expectedMiddle - rawCost - templeCost);
        gemData.Profit(ResultCase.Best).ShouldBe(expectedMiddle + 20 - rawValue);
        gemData.Profit(ResultCase.Best, null, templeCost).ShouldBe(expectedMiddle + 20 - rawValue - templeCost);
        gemData.Profit(ResultCase.Best, rawCost).ShouldBe(expectedMiddle + 20 - rawCost);
        gemData.Profit(ResultCase.Best, rawCost, templeCost).ShouldBe(expectedMiddle + 20 - rawCost - templeCost);
        gemData.Profit(value).ShouldBe(value - rawValue);
        gemData.Profit(value, null, templeCost).ShouldBe(value - rawValue - templeCost);
        gemData.Profit(value, rawCost).ShouldBe(value - rawCost);
        gemData.Profit(value, rawCost, templeCost).ShouldBe(value - rawCost - templeCost);

        gemData.Name = $"Awakened {name}";
        gemData.Profit(ResultCase.Worst).ShouldBe(expectedMiddleAwakened - 20 - rawValueAwakened);
        gemData.Profit(ResultCase.Worst, null, templeCost).ShouldBe(expectedMiddleAwakened - 20 - rawValueAwakened - templeCost);
        gemData.Profit(ResultCase.Worst, rawCost).ShouldBe(expectedMiddleAwakened - 20 - rawCost);
        gemData.Profit(ResultCase.Worst, rawCost, templeCost).ShouldBe(expectedMiddleAwakened - 20 - rawCost - templeCost);
        gemData.Profit(ResultCase.Middle).ShouldBe(expectedMiddleAwakened - rawValueAwakened);
        gemData.Profit(ResultCase.Middle, null, templeCost).ShouldBe(expectedMiddleAwakened - rawValueAwakened - templeCost);
        gemData.Profit(ResultCase.Middle, rawCost).ShouldBe(expectedMiddleAwakened - rawCost);
        gemData.Profit(ResultCase.Middle, rawCost, templeCost).ShouldBe(expectedMiddleAwakened - rawCost - templeCost);
        gemData.Profit(ResultCase.Best).ShouldBe(expectedMiddleAwakened + 20 - rawValueAwakened);
        gemData.Profit(ResultCase.Best, null, templeCost).ShouldBe(expectedMiddleAwakened + 20 - rawValueAwakened - templeCost);
        gemData.Profit(ResultCase.Best, rawCost).ShouldBe(expectedMiddleAwakened + 20 - rawCost);
        gemData.Profit(ResultCase.Best, rawCost, templeCost).ShouldBe(expectedMiddleAwakened + 20 - rawCost - templeCost);
        gemData.Profit(value).ShouldBe(value - rawValueAwakened);
        gemData.Profit(value, null, templeCost).ShouldBe(value - rawValueAwakened - templeCost);
        gemData.Profit(value, rawCost).ShouldBe(value - rawCost);
        gemData.Profit(value, rawCost, templeCost).ShouldBe(value - rawCost - templeCost);
    }

    [Fact]
    public void AvgProfitPerTryTest()
    {
        var gemData = new GemData
        {
            Name = "Test",
            Gems = new List<GemTradeData>
            {
                new() { Corrupted = true, GemLevel = 19, ChaosValue = 1 },
                new() { Corrupted = true, GemLevel = 20, ChaosValue = 2 },
                new() { Corrupted = true, GemLevel = 21, ChaosValue = 3 },
                new() { Corrupted = false, GemLevel = 20, ChaosValue = 4 }
            }
        };

        const int rawCost = 10;
        const int worstCaseValue = 11;
        const int middleCaseValue = 12;
        const int bestCaseValue = 13;
        const int templeCost = 50;

        gemData.AvgProfitPerTry().ShouldBe(-2);
        gemData.AvgProfitPerTry(templeCost: templeCost).ShouldBe(-52);
        gemData.AvgProfitPerTry(rawCost).ShouldBe(-8);
        gemData.AvgProfitPerTry(worstCaseValue: worstCaseValue).ShouldBe(0.5m);
        gemData.AvgProfitPerTry(middleCaseValue: middleCaseValue).ShouldBe(3);
        gemData.AvgProfitPerTry(bestCaseValue: bestCaseValue).ShouldBe(0.5m);
    }
}