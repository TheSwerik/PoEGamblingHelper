using Domain.Entity;
using FluentAssertions;

namespace Domain.Test;

public class TestTempleCost
{
    [Fact]
    public void TestAverageChaosValue()
    {
        var chaosValues = new[] { 1m, 2m, 3m, 4m, 5m, 6m, 7m, 8m, 9m };
        var templeCost = new TempleCost
                         {
                             Id = Guid.NewGuid(),
                             ChaosValue = chaosValues
                         };

        var expected = chaosValues.Average();
        var actual = templeCost.AverageChaosValue();

        actual.Should().Be(expected);
    }
}