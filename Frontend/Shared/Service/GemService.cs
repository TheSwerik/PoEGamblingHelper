using System.Net.Http.Json;
using Model;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService : IGemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<GemData>> GetAll()
    {
        return await _httpClient.GetFromJsonAsync<List<GemData>>("gem") ??
               throw new InvalidOperationException();
    }
}