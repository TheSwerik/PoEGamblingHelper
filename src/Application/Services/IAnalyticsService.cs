using System.Net;

namespace Application.Services;

public interface IAnalyticsService
{
    Task AddView(IPAddress? ipAddress);
    Task LogYesterdaysViews();
}