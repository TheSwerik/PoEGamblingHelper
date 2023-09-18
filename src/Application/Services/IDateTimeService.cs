namespace PoEGamblingHelper.Application.Services;

public interface IDateTimeService
{
    DateOnly UtcToday();
    DateTime UtcNow();
}