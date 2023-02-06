using Model;
using Model.QueryParameters;

namespace PoEGamblingHelper3.Shared.Service;

public interface IGemService
{
    public Task<Page<GemData>> GetAll(PageRequest? page, GemDataQuery? query);
}