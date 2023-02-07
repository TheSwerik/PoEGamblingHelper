using Model;
using Model.QueryParameters;

namespace PoEGamblingHelper3.Shared.Service;

public interface IGemService
{
    public Task<List<GemData>> GetAll(Page? page, GemDataQuery? query);
}