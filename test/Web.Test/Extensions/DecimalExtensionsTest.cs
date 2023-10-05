using System.Globalization;
using FluentAssertions;
using PoEGamblingHelper.Web.Extensions;

namespace PoEGamblingHelper.Web.Test.Extensions;

public class DecimalExtensionsTest
{
    [Theory]
    [InlineData(123.1325, 0, 3)]
    [InlineData(123.1325, 1, 5)]
    [InlineData(123.1325, 2, 6)]
    [InlineData(123.1325, 3, 7)]
    [InlineData(123.1325, 4, 8)]
    [InlineData(123.1325, 5, 8)]
    public void RoundTest(decimal value, int places, int expectedLength)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        var result = value.Round(places);

        result.Length.Should().Be(expectedLength);
        int.Parse(result.Split('.')[0]).Should().Be((int)value);
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
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
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
            int.Parse(result.Split('.')[0]).Should().Be((int)decimalValue!);
        }
    }
}