﻿using Domain.Entity.Gem;

namespace Domain.Util;

public static class ExtensionMethods
{
    public static int LevelModifier(this ResultCase resultCase) { return (int)resultCase - 1; }
}