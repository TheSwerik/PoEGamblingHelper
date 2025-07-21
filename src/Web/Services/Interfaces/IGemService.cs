using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IGemService
{
    public Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query);
}