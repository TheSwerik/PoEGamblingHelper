using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public interface ILeagueService
{
    public Task<League?> GetCurrent();
}