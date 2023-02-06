using Blazored.Toast.Services;
using Shared.Entity;
using Shared.QueryParameters;

namespace PoEGamblingHelper3.Service;

public class GemService : Service, IGemService
{
    public GemService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }

    public async Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await GetAsync<Page<GemData>>("gem" + queryString);
    }
}