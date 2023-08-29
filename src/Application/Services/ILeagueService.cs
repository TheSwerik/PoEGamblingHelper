using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public interface ILeagueService
{
    League GetCurrentLeague();
}