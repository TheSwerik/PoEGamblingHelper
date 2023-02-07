using Blazored.Toast.Services;
using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public class CurrencyService : Service, ICurrencyService
{
    public CurrencyService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<List<Currency>?> GetAll() { return await GetAsync<List<Currency>>("currency"); }
}