using FluentAssertions;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Web.Shared.Model;

namespace PoEGamblingHelper.Web.Test.Shared.Model;

public class FilterValuesTest
{
    [Fact]
    public void ToQuery()
    {
        var source = new FilterValues
                     {
                         Gem = "Empower Support",
                         Sort = Sort.CostPerTryDesc,
                         GemType = GemType.Exceptional,
                         OnlyShowProfitable = true,
                         ShowAlternateQuality = true,
                         PricePerTryFrom = 0m,
                         PricePerTryTo = 654m
                     };

        var result = source.ToQuery();

        result.SearchText.Should().Be(source.Gem);
        result.Sort.Should().Be(source.Sort);
        result.GemType.Should().Be(source.GemType);
        result.OnlyShowProfitable.Should().Be(source.OnlyShowProfitable);
        result.ShowAlternateQuality.Should().Be(source.ShowAlternateQuality);
        result.PricePerTryFrom.Should().Be(source.PricePerTryFrom);
        result.PricePerTryTo.Should().Be(source.PricePerTryTo);
        result.ShowVaal.Should().BeFalse();
    }
}