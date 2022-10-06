using System.Net.Http.Json;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<IEnumerable<Gem>> GetAllGems()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Gem>>("data") ?? throw new InvalidOperationException();
    }
}