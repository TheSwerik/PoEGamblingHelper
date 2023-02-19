using Domain.Entity.Gem;
using FluentAssertions;

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
        gemData.IsExceptional().Should().Be(expected);

        gemData.Name = name.ToLowerInvariant();
        gemData.IsExceptional().Should().Be(expected);

        gemData.Name = $"Awakened {name}";
        gemData.IsExceptional().Should().Be(expected);

        gemData.Name = $"Awakened {name.ToLowerInvariant()}";
        gemData.IsExceptional().Should().Be(expected);
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
        gemData.MaxLevel().Should().Be(expected);

        gemData.Name = name.ToLowerInvariant();
        gemData.MaxLevel().Should().Be(expected);

        gemData.Name = $"Awakened {name}";
        gemData.MaxLevel().Should().Be(expectedAwakened);

        gemData.Name = $"Awakened {name.ToLowerInvariant()}";
        gemData.MaxLevel().Should().Be(expectedAwakened);
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
        gemData.RawCost().Should().Be(expected);

        gemData.Name = $"Awakened {name}";
        gemData.RawCost().Should().Be(expectedAwakened);
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
        gemData.CostPerTry().Should().Be(expected);
        gemData.CostPerTry().Should().Be(expected);
        gemData.CostPerTry(rawCost).Should().Be(rawCost);
        gemData.CostPerTry(templeCost: templeCost).Should().Be(expected + templeCost);
        gemData.CostPerTry(null, templeCost).Should().Be(expected + templeCost);
        gemData.CostPerTry(rawCost, templeCost).Should().Be(rawCost + templeCost);

        gemData.Name = $"Awakened {name}";
        gemData.CostPerTry().Should().Be(expectedAwakened);
        gemData.CostPerTry().Should().Be(expectedAwakened);
        gemData.CostPerTry(rawCost).Should().Be(rawCost);
        gemData.CostPerTry(templeCost: templeCost).Should().Be(expectedAwakened + templeCost);
        gemData.CostPerTry(null, templeCost).Should().Be(expectedAwakened + templeCost);
        gemData.CostPerTry(rawCost, templeCost).Should().Be(rawCost + templeCost);
    }
    //TODO more tests
}