using System.Net.Http.Json;
using Model;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService : IGemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<GemData>> GetAllGems()
    {
        var result = await _httpClient.GetAsync("data");
        Console.WriteLine(result.Headers.Location);
        return await _httpClient.GetFromJsonAsync<List<GemData>>("data") ??
               throw new InvalidOperationException();
    }
}