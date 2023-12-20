using FluentAssertions;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Web.Pages.GamblingHelper.Components.Filter;

namespace PoEGamblingHelper.Web.Test.Shared.Model;

public class FilterValuesTest
{
    [Fact]
    public void ToQuery()
    {
        var source = new FilterModel
                     {
                         Gem = "Empower Support",
                         Sort = Sort.CostPerTryDesc,
                         GemType = GemType.Exceptional,
                         OnlyShowProfitable = true,
                         PricePerTryFrom = 0m,
                         PricePerTryTo = 654m
                     };

        var result = source.ToQuery();

        result.SearchText.Should().Be(source.Gem);
        result.Sort.Should().Be(source.Sort);
        result.GemType.Should().Be(source.GemType);
        result.OnlyShowProfitable.Should().Be(source.OnlyShowProfitable);
        result.PricePerTryFrom.Should().Be(source.PricePerTryFrom);
        result.PricePerTryTo.Should().Be(source.PricePerTryTo);
        result.ShowVaal.Should().BeFalse();
    }
}