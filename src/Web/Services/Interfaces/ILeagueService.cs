using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface ILeagueService
{
    public Task<League?> GetCurrent();
}