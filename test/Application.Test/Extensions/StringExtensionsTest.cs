using FluentAssertions;
using PoEGamblingHelper.Application.Extensions;

namespace PoEGamblingHelper.Application.Test.Extensions;

public class StringExtensionsTest
{
    [Theory]
    [InlineData("test", "test", true)]
    [InlineData("tEst", "teSt", true)]
    [InlineData("TEST", "TEST", true)]
    [InlineData("testa", "test", false)]
    [InlineData(null, "test", false)]
    [InlineData("test", null, false)]
    [InlineData(null, null, true)]
    public void EqualsIgnoreCaseTest(string? a, string? b, bool isTrue)
    {
        a.EqualsIgnoreCase(b).Should().Be(isTrue);
        b.EqualsIgnoreCase(a).Should().Be(isTrue);
        a.EqualsIgnoreCase(a).Should().BeTrue();
        b.EqualsIgnoreCase(b).Should().BeTrue();
    }
}