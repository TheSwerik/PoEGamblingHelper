using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface ILeagueService
{
    Task<League?> GetCurrent();
    Task<string[]> GetCurrentLeagues();
}