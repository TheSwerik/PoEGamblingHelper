using Shared.Entity;
using Shared.QueryParameters;

namespace Backend.Data;

public interface IGemDataRepository : IRepository<GemData, Guid>
{
    Page<GemData> GetAll(GemDataQuery? query, PageRequest page);
}