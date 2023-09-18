using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Api.Controllers;

public class GemController : ApiControllerBase
{
    private readonly IGemRepository _gemRepository;

    public GemController(IGemRepository gemRepository) { _gemRepository = gemRepository; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public async Task<Page<GemData>> GetAll([FromQuery] GemDataQuery? query, [FromQuery] PageRequest page)
    {
        return await _gemRepository.Search(query, page);
    }
}