﻿using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Repositories;

public interface ILeagueRepository
{
    League GetByStartDateBefore(DateTime dateTime);
    IAsyncEnumerable<League> GetAllLeagues();
    League GetCurrent();
}