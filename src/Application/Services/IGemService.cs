using Domain.Entity.Gem;
using Domain.QueryParameters;

namespace Application.Services;

public interface IGemService
{
    Page<GemData> GetAll(GemDataQuery? query, PageRequest page);
}