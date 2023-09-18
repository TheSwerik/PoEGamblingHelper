using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure;

namespace PoEGamblingHelper.Api.Controllers;

public class GemController(IGemRepository gemRepository) : ApiControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = Constants.DataFetcherCacheTag)]
    public async Task<Page<GemData>> GetAll([FromQuery] GemDataQuery? query, [FromQuery] PageRequest page)
    {
        return await gemRepository.Search(query, page);
    }
}