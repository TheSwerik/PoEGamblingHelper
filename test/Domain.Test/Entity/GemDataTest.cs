using Domain.Entity.Gem;
using FluentAssertions;

namespace Domain.Test.Entity;

public class GemDataTest
{
    [Fact]
    public void IsExceptionalTest()
    {
        var names = new[] { "Enhance", "Empower", "Enlighten" };
        var gemData = new GemData();

        foreach (var name in names)
        {
            gemData.Name = $"{name} Support";
            gemData.IsExceptional().Should().BeTrue();

            gemData.Name = $"{name.ToLowerInvariant()} Support";
            gemData.IsExceptional().Should().BeTrue();

            gemData.Name = $"Awakened {name} Support";
            gemData.IsExceptional().Should().BeTrue();

            gemData.Name = $"Awakened {name.ToLowerInvariant()} Support";
            gemData.IsExceptional().Should().BeTrue();
        }
    }

    [Fact]
    public void MaxLevelTest()
    {
        var exceptionalNames = new[] { "Enhance", "Empower", "Enlighten" };
        var gemData = new GemData();

        foreach (var name in exceptionalNames)
        {
            gemData.Name = $"{name} Support";
            gemData.MaxLevel().Should().Be(3);

            gemData.Name = $"{name.ToLowerInvariant()} Support";
            gemData.MaxLevel().Should().Be(3);

            gemData.Name = $"Awakened {name} Support";
            gemData.MaxLevel().Should().Be(4);

            gemData.Name = $"Awakened {name.ToLowerInvariant()} Support";
            gemData.MaxLevel().Should().Be(4);
        }

        gemData.Name = "Awakened Test";
        gemData.MaxLevel().Should().Be(5);

        gemData.Name = "awakened Test";
        gemData.MaxLevel().Should().Be(5);

        gemData.Name = "Test";
        gemData.MaxLevel().Should().Be(20);

        gemData.Name = "test";
        gemData.MaxLevel().Should().Be(20);
    }

    [Fact]
    public void RawCostTest()
    {
        var exceptionalNames = new[] { "Enhance", "Empower", "Enlighten" };
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

        foreach (var name in exceptionalNames)
        {
            gemData.Name = $"{name} Support";
            gemData.RawCost().Should().Be(3);
            gemData.Name = $"Awakened {name} Support";
            gemData.RawCost().Should().Be(6);
        }

        gemData.Name = "Awakened Test";
        gemData.RawCost().Should().Be(8);

        gemData.Name = "Test";
        gemData.RawCost().Should().Be(12);
    }
    //TODO more tests
}