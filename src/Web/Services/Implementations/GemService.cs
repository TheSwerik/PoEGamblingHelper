using Blazored.Toast.Services;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using Web.Services.Interfaces;

namespace Web.Services.Implementations;

public class GemService : HttpService, IGemService
{
    public GemService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }

    public async Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await GetAsync<Page<GemData>>("gem" + queryString);
    }
}