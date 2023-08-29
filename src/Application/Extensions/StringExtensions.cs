namespace PoEGamblingHelper.Application.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? a, string? b)
    {
        return a?.Equals(b, StringComparison.InvariantCultureIgnoreCase)
               ?? b is null;
    }
}