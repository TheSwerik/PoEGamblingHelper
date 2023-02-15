using Domain.Entity.Gem;
using Domain.QueryParameters;

namespace Application.Service;

public interface IGemService
{
    Page<GemData> GetAll(GemDataQuery? query, PageRequest page);
}