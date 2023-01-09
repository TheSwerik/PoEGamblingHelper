using System.Net.Http.Json;
using Model;
using Model.QueryParameters;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService : IGemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<GemData>> GetAll(Page? page, GemDataQuery? query)
    {
        var queryString = query?.ToQueryString(page) ?? page?.ToQueryString() ?? "";
        return await _httpClient.GetFromJsonAsync<List<GemData>>("gem" + queryString) ??
               throw new InvalidOperationException();
    }
}