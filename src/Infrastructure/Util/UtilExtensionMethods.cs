namespace Infrastructure.Util;

public static class UtilExtensionMethods
{
    public static bool EqualsIgnoreCase(this string? a, string? b)
    {
        return a?.Equals(b, StringComparison.InvariantCultureIgnoreCase)
               ?? b is null;
    }
}