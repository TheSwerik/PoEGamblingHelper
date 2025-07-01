using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure.DataFetcher;
using PoEGamblingHelper.Infrastructure.Extensions;

namespace PoEGamblingHelper.Infrastructure.Test.Extensions;

public class GemDataExtensionsTest
{
    [Fact]
    public void ToGemDataTest()
    {
        const string empower = "Empower Support";
        const string enlighten = "Enlighten Support";
        const string empowerIcon = "Empower Icon";
        const string enlightenIcon = "Enlighten Icon";
        var list = new List<PoeNinjaGemData>
        {
            new(0, empower, empowerIcon, 1, 1, false, "", 1, 1, 1, 1),
            new(1, enlighten, enlightenIcon, 2, 2, false, "", 1, 1, 1, 1),
            new(2, empower, empowerIcon, 3, 3, false, "", 1, 1, 1, 1),
            new(3, empower, empowerIcon, 4, 4, false, "", 1, 1, 1, 1),
            new(4, enlighten, enlightenIcon, 5, 5, false, "", 1, 1, 1, 1)
        };
        var group = list.GroupBy(p => p.Name).First();
        var gemTradeData = new List<GemTradeData>
        {
            new() { Name = empower },
            new() { Name = empower },
            new() { Name = enlighten },
            new() { Name = empower },
            new() { Name = enlighten },
            new() { Name = enlighten }
        };

        var result = group.ToGemData(gemTradeData);

        result.Id.ShouldBe(Guid.Empty);
        result.Name.ShouldBe(empower);
        result.Icon.ShouldBe(empowerIcon);
        result.Gems.ShouldBeEquivalentTo(gemTradeData.Where(g => g.Name.EqualsIgnoreCase(empower)).ToList());
    }

    [Fact]
    public void ToGemDataWithExistingGemDataTest()
    {
        const string empower = "Empower Support";
        const string enlighten = "Enlighten Support";
        const string empowerIcon = "Empower Icon";
        const string enlightenIcon = "Enlighten Icon";
        var list = new List<PoeNinjaGemData>
        {
            new(0, empower, empowerIcon, 1, 1, false, "", 1, 1, 1, 1),
            new(1, enlighten, enlightenIcon, 2, 2, false, "", 1, 1, 1, 1),
            new(2, empower, empowerIcon, 3, 3, false, "", 1, 1, 1, 1),
            new(3, empower, empowerIcon, 4, 4, false, "", 1, 1, 1, 1),
            new(4, enlighten, enlightenIcon, 5, 5, false, "", 1, 1, 1, 1)
        };
        var group = list.GroupBy(p => p.Name).First();
        var gemTradeData = new List<GemTradeData>
        {
            new() { Name = empower },
            new() { Name = empower },
            new() { Name = enlighten },
            new() { Name = empower },
            new() { Name = enlighten },
            new() { Name = enlighten }
        };
        var existingGemData = new List<GemData>
        {
            new() { Name = enlighten, Id = Guid.Parse("00000000-0000-0000-0000-000000000002") },
            new() { Name = empower, Id = Guid.Parse("00000000-0000-0000-0000-000000000001") }
        };

        var result = group.ToGemData(gemTradeData, existingGemData);

        result.Id.ShouldBe(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        result.Name.ShouldBe(empower);
        result.Icon.ShouldBe(empowerIcon);
        result.Gems.ShouldBeEquivalentTo(gemTradeData.Where(g => g.Name.EqualsIgnoreCase(empower)).ToList());
    }
}