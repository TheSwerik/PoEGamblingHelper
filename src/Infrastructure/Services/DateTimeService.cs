using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateOnly UtcToday() { return DateOnly.FromDateTime(UtcNow()); }

    public DateTime UtcNow() { return DateTime.UtcNow; }
}