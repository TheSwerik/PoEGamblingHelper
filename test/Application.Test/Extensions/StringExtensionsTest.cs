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
        a.EqualsIgnoreCase(b).ShouldBe(isTrue);
        b.EqualsIgnoreCase(a).ShouldBe(isTrue);
        a.EqualsIgnoreCase(a).ShouldBeTrue();
        b.EqualsIgnoreCase(b).ShouldBeTrue();
    }
}