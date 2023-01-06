using Model;
using Model.QueryParameters;

namespace Backend.Data;

public interface IGemDataRepository : IRepository<GemData, Guid>
{
    IEnumerable<GemData> GetAll(GemDataQuery? query, Page? page);
}