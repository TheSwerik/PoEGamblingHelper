using Domain.Entity.Gem;
using Domain.Util;

namespace Domain.Test.Util;

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