using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class CurrencyService : HttpService, ICurrencyService
{
    public CurrencyService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<List<Currency>?> GetAll() { return await GetAsync<List<Currency>>("currency"); }
}