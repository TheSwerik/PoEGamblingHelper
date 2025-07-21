using Blazored.Toast.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class LeagueService(HttpClient httpClient, IToastService toastService) : HttpService(httpClient, toastService),
                                                                                ILeagueService
{
    public async Task<League?> GetCurrent()
    {
        return await GetAsync<League>("league/current");
    }

    public async Task<string[]> GetCurrentLeagues()
    {
        var currentLeague = await GetCurrent();
        if (currentLeague is null) return [];
        return
        [
            currentLeague.Name,
            $"Hardcore {currentLeague.Name}",
            "Standard",
            "Hardcore"
        ];
    }
}