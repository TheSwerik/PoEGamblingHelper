using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IAnalyticsService
{
    Task<List<AnalyticsDay>?> GetAll();
    Task<List<AnalyticsDay>?> Get(DateTime start, DateTime end);
    Task<bool> Check();
    Task Login(string password);
}