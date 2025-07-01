using Domain.Entity.Gem;
using Infrastructure.Services.FetchDtos;
using Infrastructure.Util;

namespace Infrastructure.Test.Util;

public class ExtensionMethodsTest
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

    [Fact]
    public void ToGemDataTest()
    {
        const string empower = "Empower Support";
        const string enlighten = "Enlighten Support";
        const string empowerIcon = "Empower Icon";
        const string enlightenIcon = "Enlighten Icon";
        var list = new List<PoeNinjaGemData>
        {
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = enlighten, Icon = enlightenIcon },
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = enlighten, Icon = enlightenIcon }
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
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = enlighten, Icon = enlightenIcon },
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = empower, Icon = empowerIcon },
            new() { Name = enlighten, Icon = enlightenIcon }
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