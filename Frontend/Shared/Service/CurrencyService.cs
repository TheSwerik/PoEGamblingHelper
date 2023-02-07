using System.Net.Http.Json;
using Model;

namespace PoEGamblingHelper3.Shared.Service;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService(HttpClient httpClient) { _httpClient = httpClient; }

    public async Task<List<Currency>> GetAll()
    {
        return await _httpClient.GetFromJsonAsync<List<Currency>>("currency") ??
               throw new InvalidOperationException();
    }
}