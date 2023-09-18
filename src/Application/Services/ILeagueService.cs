using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public interface ILeagueService //TODO rename
{
    League GetCurrentLeague();
}