using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure;

namespace PoEGamblingHelper.Api.Controllers;

public class TempleController(ITempleRepository templeRepository) : ApiControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = Constants.DataFetcherCacheTag)]
    public TempleCost Get([FromQuery] string league)
    {
        return templeRepository.GetCurrent(league);
    }
}