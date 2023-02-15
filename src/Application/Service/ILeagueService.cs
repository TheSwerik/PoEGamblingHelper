using Domain.Entity;

namespace Application.Service;

public interface ILeagueService
{
    IAsyncEnumerable<League> GetAllAsync();
    League? GetCurrentLeague();
}