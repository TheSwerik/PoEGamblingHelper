using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Shared.QueryParameters;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IGemService
{
    public Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query);
}