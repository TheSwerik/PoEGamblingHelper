using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class TempleCostService : HttpService, ITempleCostService
{
    public TempleCostService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<TempleCost?> Get() { return await GetAsync<TempleCost>("temple"); }
}