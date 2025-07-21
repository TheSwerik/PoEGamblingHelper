using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Web.Extensions;

namespace PoEGamblingHelper.Web.Test.Extensions;

public class StringifyExtensionsTest
{
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