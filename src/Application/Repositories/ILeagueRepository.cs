using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Repositories;

public interface ILeagueRepository
{
    League GetByStartDateAfter(DateTime dateTime);
}