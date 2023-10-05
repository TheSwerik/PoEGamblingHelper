using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class CurrencyService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService),
    ICurrencyService
{
    public async Task<List<Currency>?> GetAll() { return await GetAsync<List<Currency>>("currency"); }
}