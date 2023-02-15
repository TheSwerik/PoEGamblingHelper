using Domain.Entity;

namespace Application.Services;

public interface ILeagueService
{
    League GetCurrentLeague(IApplicationDbContext applicationDbContext);
}