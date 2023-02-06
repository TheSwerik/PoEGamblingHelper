using Blazored.Toast.Services;
using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public class TempleCostService : Service, ITempleCostService
{
    public TempleCostService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<TempleCost?> Get() { return await GetAsync<TempleCost>("temple"); }
}