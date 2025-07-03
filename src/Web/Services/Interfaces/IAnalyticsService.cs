using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IAnalyticsService
{
    public Task<List<AnalyticsDay>?> GetAll();
    public Task<List<AnalyticsDay>?> Get(DateTime start, DateTime end);
}