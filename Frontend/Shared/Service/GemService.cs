using System.Net.Http.Json;
using Model;
using Model.QueryParameters;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService : IGemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<Page<GemData>> GetAll(PageRequest? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await _httpClient.GetFromJsonAsync<Page<GemData>>("gem" + queryString) ??
               throw new InvalidOperationException();
    }
}