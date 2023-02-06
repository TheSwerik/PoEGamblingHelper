using Shared.Entity;

namespace Shared.Util;

public static class ExtensionMethods
{
    public static int LevelModifier(this ResultCase resultCase) { return (int)resultCase - 1; }

    public static bool EqualsIgnoreCase(this string a, string? b)
    {
        return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
    }
}