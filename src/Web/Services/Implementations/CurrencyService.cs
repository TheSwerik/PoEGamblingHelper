using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using Web.Services.Interfaces;

namespace Web.Services.Implementations;

public class CurrencyService : HttpService, ICurrencyService
{
    public CurrencyService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<List<Currency>?> GetAll() { return await GetAsync<List<Currency>>("currency"); }
}