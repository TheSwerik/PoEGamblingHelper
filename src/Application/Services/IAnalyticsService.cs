namespace Application.Services;

public interface IAnalyticsService
{
    Task AddView(string? ipAddress);
    Task LogYesterdaysViews();
}