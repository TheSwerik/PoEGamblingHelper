using Blazored.Toast.Services;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Web.Extensions;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class GemService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService),
                                                                             IGemService
{
    public async Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await GetAsync<Page<GemData>>("gem" + queryString);
    }
}