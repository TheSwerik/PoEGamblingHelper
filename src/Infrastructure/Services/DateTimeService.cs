using System.Diagnostics.CodeAnalysis;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.Services;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class DateTimeService : IDateTimeService
{
    public DateOnly UtcToday() { return DateOnly.FromDateTime(UtcNow()); }

    public DateTime UtcNow() { return DateTime.UtcNow; }
}