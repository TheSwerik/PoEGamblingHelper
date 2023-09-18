﻿using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Application.Services;

public interface IGemService
{
    Task<Page<GemData>> GetAll(GemDataQuery? query, PageRequest page);
}