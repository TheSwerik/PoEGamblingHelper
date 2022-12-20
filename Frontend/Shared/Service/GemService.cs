﻿using System.Net.Http.Json;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared.Service;

public class GemService : IGemService
{
    private readonly HttpClient _httpClient;

    public GemService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<IEnumerable<Gem>> GetAllGems()
    {
        var result = await _httpClient.GetAsync("data");
        Console.WriteLine(result.Headers.Location);
        Console.WriteLine(await result.Content.ReadAsStringAsync());
        return await _httpClient.GetFromJsonAsync<IEnumerable<Gem>>("data") ?? throw new InvalidOperationException();
    }
}