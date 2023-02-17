using Blazored.Toast.Services;
using Domain.Entity;
using Web.Services.Interfaces;

namespace Web.Services.Implementations;

public class TempleCostService : HttpService, ITempleCostService
{
    public TempleCostService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<TempleCost?> Get() { return await GetAsync<TempleCost>("temple"); }
}