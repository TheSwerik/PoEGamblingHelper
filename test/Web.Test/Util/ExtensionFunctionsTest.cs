using FluentAssertions;
using Web.Util;
using Xunit.Abstractions;

namespace Web.Test.Util;

public class ExtensionFunctionsTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    public ExtensionFunctionsTest(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

    [Theory]
    [InlineData(123.1325, 0, 3)]
    [InlineData(123.1325, 1, 5)]
    [InlineData(123.1325, 2, 6)]
    [InlineData(123.1325, 3, 7)]
    [InlineData(123.1325, 4, 8)]
    [InlineData(123.1325, 5, 8)]
    public void RoundTest(decimal value, int places, int expectedLength)
    {
        var result = value.Round(places);

        result.Length.Should().Be(expectedLength);
        int.Parse(result.Split(',')[0]).Should().Be((int)value);
    }

    [Theory]
    [InlineData(null, 5, null)]
    [InlineData(123.1325, 0, 3)]
    [InlineData(123.1325, 1, 5)]
    [InlineData(123.1325, 2, 6)]
    [InlineData(123.1325, 3, 7)]
    [InlineData(123.1325, 4, 8)]
    [InlineData(123.1325, 5, 8)]
    public void RoundNullableTest(double? value, int places, int? expectedLength)
    {
        var decimalValue = (decimal?)value;
        var result = decimalValue.Round(places);

        if (expectedLength is null)
        {
            result.Should().BeNull();
        }
        else
        {
            result.Should().NotBeNull();
            result!.Length.Should().Be(expectedLength);
            int.Parse(result.Split(',')[0]).Should().Be((int)decimalValue!);
        }
    }

    [Fact] public void SortToPrettyStringTest() { Assert.Fail("Not Implemented"); }
    [Fact] public void GemTypeToPrettyStringTest() { Assert.Fail("Not Implemented"); }
    [Fact] public void ToQueryStringTest() { Assert.Fail("Not Implemented"); }
    [Fact] public void TradeUrlTest() { Assert.Fail("Not Implemented"); }
    [Fact] public void TradeQueryTest() { Assert.Fail("Not Implemented"); }
}