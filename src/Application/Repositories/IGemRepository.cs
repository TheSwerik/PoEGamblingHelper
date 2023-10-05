using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Application.Repositories;

public interface IGemRepository
{
    Page<GemData> Search(GemDataQuery? query, PageRequest page);
}