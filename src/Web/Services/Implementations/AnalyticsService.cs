using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class AnalyticsService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService),
                                                                                   IAnalyticsService
{
    public async Task<List<AnalyticsDay>?> Get(DateTime start, DateTime end)
    {
        return await GetAsync<List<AnalyticsDay>?>($"analytics?start={DateOnly.FromDateTime(start)}&end={DateOnly.FromDateTime(end)}");
    }

    public async Task<bool> Check()
    {
        return await GetAsync<bool>("analytics/check");
    }

    public async Task Login(string password)
    {
        await GetAsync("analytics/login", ("Authorization", $"Bearer {password}"));
    }
}