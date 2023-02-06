using Shared.Entity;
using Shared.QueryParameters;

namespace PoEGamblingHelper3.Service;

public interface IGemService
{
    public Task<Page<GemData>?> GetAll(PageRequest? page, GemDataQuery? query);
}