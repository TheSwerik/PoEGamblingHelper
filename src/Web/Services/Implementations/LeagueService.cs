using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class LeagueService : HttpService, ILeagueService
{
    public LeagueService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<League?> GetCurrent() { return await GetAsync<League>("league/current"); }
}