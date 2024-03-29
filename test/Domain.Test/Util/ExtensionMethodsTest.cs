﻿using Domain.Entity.Gem;
using Domain.Util;
using FluentAssertions;

namespace Domain.Test.Util;

public class ExtensionMethodsTest
{
    [Fact]
    public void LevelModifierTest()
    {
        ResultCase.Worst.LevelModifier().Should().Be(-1);
        ResultCase.Middle.LevelModifier().Should().Be(0);
        ResultCase.Best.LevelModifier().Should().Be(1);
    }
}