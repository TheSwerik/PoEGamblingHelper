using System.Globalization;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Web.Extensions;

namespace PoEGamblingHelper.Web.Test.Util;

public class ExtensionFunctionsTest
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
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        var result = value.Round(places);

        result.Length.ShouldBe(expectedLength);
        int.Parse(result.Split('.')[0]).ShouldBe((int)value);
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
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        var decimalValue = (decimal?)value;
        var result = decimalValue.Round(places);

        if (expectedLength is null)
        {
            result.ShouldBeNull();
        }
        else
        {
            result.ShouldNotBeNull();
            result.Length.ShouldBe(expectedLength.Value);
            int.Parse(result.Split('.')[0]).ShouldBe((int)decimalValue!);
        }
    }

    [Theory]
    [InlineData(Sort.CostPerTryAsc, "Cost Ascending")]
    [InlineData(Sort.CostPerTryDesc, "Cost Descending")]
    [InlineData(Sort.AverageProfitPerTryAsc, "Average profit per try Ascending")]
    [InlineData(Sort.AverageProfitPerTryDesc, "Average profit per try Descending")]
    [InlineData(Sort.MaxProfitPerTryAsc, "Maximum profit per try Ascending")]
    [InlineData(Sort.MaxProfitPerTryDesc, "Maximum profit per try Descending")]
    public void SortToPrettyStringTest(Sort sort, string prettyString)
    {
        sort.ToPrettyString().ShouldBe(prettyString);
    }

    [Theory]
    [InlineData(GemType.All, "All Gems")]
    [InlineData(GemType.Awakened, "Awakened Support Gem")]
    [InlineData(GemType.Exceptional, "Exceptional Support Gem")]
    [InlineData(GemType.Skill, "Skill Gem")]
    [InlineData(GemType.RegularSupport, "Regular Support Gem")]
    public void GemTypeToPrettyStringTest(GemType gemType, string prettyString)
    {
        gemType.ToPrettyString().ShouldBe(prettyString);
    }
}