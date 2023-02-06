using Blazored.Toast.Services;
using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public class LeagueService : Service, ILeagueService
{
    public LeagueService(HttpClient httpClient, IToastService toastService) : base(httpClient, toastService) { }
    public async Task<League?> GetCurrent() { return await GetAsync<League>("league/current"); }
}