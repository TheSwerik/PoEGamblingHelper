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

        result.SearchText.ShouldBe(source.Gem);
        result.Sort.ShouldBe(source.Sort);
        result.GemType.ShouldBe(source.GemType);
        result.OnlyShowProfitable.ShouldBe(source.OnlyShowProfitable);
        result.PricePerTryFrom.ShouldBe(source.PricePerTryFrom);
        result.PricePerTryTo.ShouldBe(source.PricePerTryTo);
        result.ShowVaal.ShouldBeFalse();
    }
}