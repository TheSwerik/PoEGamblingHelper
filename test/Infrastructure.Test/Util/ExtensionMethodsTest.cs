using FluentAssertions;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure.Services.FetchDtos;
using PoEGamblingHelper.Infrastructure.Util;

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
        a.EqualsIgnoreCase(b).Should().Be(isTrue);
        b.EqualsIgnoreCase(a).Should().Be(isTrue);
        a.EqualsIgnoreCase(a).Should().BeTrue();
        b.EqualsIgnoreCase(b).Should().BeTrue();
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

        result.Id.Should().Be(Guid.Empty);
        result.Name.Should().Be(empower);
        result.Icon.Should().Be(empowerIcon);
        result.Gems.Should().BeEquivalentTo(gemTradeData.Where(g => g.Name.EqualsIgnoreCase(empower)));
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

        result.Id.Should().Be(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        result.Name.Should().Be(empower);
        result.Icon.Should().Be(empowerIcon);
        result.Gems.Should().BeEquivalentTo(gemTradeData.Where(g => g.Name.EqualsIgnoreCase(empower)));
    }
}