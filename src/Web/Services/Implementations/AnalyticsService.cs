using Blazored.Toast.Services;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Analytics;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Web.Extensions;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class AnalyticsService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService),
                                                                                   IAnalyticsService
{
    public async Task<List<AnalyticsDay>?> GetAll()
    {
        return await GetAsync<List<AnalyticsDay>?>("analytics");
    }

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

    public async Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await GetAsync<Page<GemData>>("gem" + queryString);
    }
}