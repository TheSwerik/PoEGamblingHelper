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

    public async Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await GetAsync<Page<GemData>>("gem" + queryString);
    }
}