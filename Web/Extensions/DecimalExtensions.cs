namespace PoEGamblingHelper.Web.Extensions;

public static class DecimalExtensions
{
    public static string Round(this decimal value, int places)
    {
        var placeCharacters = new string('#', places);
        return string.Format($"{{0:0.{placeCharacters}}}", value);
    }

    public static string? Round(this decimal? value, int places)
    {
        var placeCharacters = new string('#', places);
        return value is null ? null : string.Format($"{{0:0.{placeCharacters}}}", value);
    }
}