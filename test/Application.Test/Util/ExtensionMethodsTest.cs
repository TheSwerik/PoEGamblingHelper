using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Application.Test.Util;

public class ExtensionMethodsTest
{
    [Fact]
    public void LevelModifierTest()
    {
        ResultCase.Worst.LevelModifier().ShouldBe(-1);
        ResultCase.Middle.LevelModifier().ShouldBe(0);
        ResultCase.Best.LevelModifier().ShouldBe(1);
    }
}