using Model;

namespace PoEGamblingHelper3.Shared.Service;

public interface ILeagueService
{
    public Task<League> GetCurrent();
}