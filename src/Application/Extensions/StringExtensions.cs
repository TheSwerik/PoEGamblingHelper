namespace PoEGamblingHelper.Application.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? source, string? value)
    {
        return source?.Equals(value, StringComparison.InvariantCultureIgnoreCase)
               ?? value is null;
    }
}