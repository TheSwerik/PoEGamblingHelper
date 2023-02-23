using Domain.Entity.Gem;
using Domain.QueryParameters;

namespace Web.Services.Interfaces;

public interface IGemService
{
    public Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query);
}