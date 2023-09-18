namespace PoEGamblingHelper.Application.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToUtcDateTime(this DateOnly dateOnly)
    {
        return dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
    }
}