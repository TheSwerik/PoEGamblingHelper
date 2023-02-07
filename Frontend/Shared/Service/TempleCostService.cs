using System.Net.Http.Json;
using Model;

namespace PoEGamblingHelper3.Shared.Service;

public class TempleCostService : ITempleCostService
{
    private readonly HttpClient _httpClient;

    public TempleCostService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<TempleCost> Get()
    {
        return await _httpClient.GetFromJsonAsync<TempleCost>("temple") ??
               throw new InvalidOperationException();
    }
}