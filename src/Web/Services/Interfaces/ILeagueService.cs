using Domain.Entity;

namespace Web.Services.Interfaces;

public interface ILeagueService
{
    public Task<League?> GetCurrent();
}