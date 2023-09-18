using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Api.Controllers;

public class GemController : ApiControllerBase
{
    private readonly IGemService _gemService;

    public GemController(IGemService gemService) { _gemService = gemService; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public async Task<Page<GemData>> GetAll([FromQuery] GemDataQuery? query, [FromQuery] PageRequest page)
    {
        return await _gemService.GetAll(query, page);
    }
}