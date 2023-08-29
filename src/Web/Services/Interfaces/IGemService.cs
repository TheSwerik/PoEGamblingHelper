using PoEGamblingHelper.Domain.Entity.Gem;

namespace Web.Services.Interfaces;

public interface IGemService
{
    public Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query);
}