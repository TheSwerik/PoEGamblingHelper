using System.Net.Http.Json;
using Model;

namespace PoEGamblingHelper3.Shared.Service;

public class LeagueService : ILeagueService
{
    private readonly HttpClient _httpClient;

    public LeagueService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<League> GetCurrent()
    {
        return await _httpClient.GetFromJsonAsync<League>("league/current") ??
               throw new InvalidOperationException();
    }
}