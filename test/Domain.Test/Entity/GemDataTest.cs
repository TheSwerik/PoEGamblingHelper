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
    //TODO more tests
}