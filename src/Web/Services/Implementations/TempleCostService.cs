using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class TempleCostService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService), ITempleCostService
{
    public async Task<TempleCost?> Get(string league)
    {
        return await GetAsync<TempleCost>($"temple?league={league}");
    }
}