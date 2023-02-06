using Model;
using Model.QueryParameters;

namespace Backend.Data;

public interface IGemDataRepository : IRepository<GemData, Guid>
{
    Page<GemData> GetAll(GemDataQuery? query, PageRequest page);
}