namespace PoEGamblingHelper.Application.Services;

public interface IAnalyticsService //TODO rename
{
    Task AddView(string? ipAddress);
    Task LogYesterdaysViews();
}