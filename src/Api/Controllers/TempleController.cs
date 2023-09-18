using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure;

namespace PoEGamblingHelper.Api.Controllers;

public class TempleController : ApiControllerBase
{
    private readonly ITempleRepository _templeRepository;

    public TempleController(ITempleRepository templeRepository) { _templeRepository = templeRepository; }

    [HttpGet]
    [OutputCache(PolicyName = Constants.DataFetcherCacheTag)]
    public TempleCost Get() { return _templeRepository.GetCurrent(); }
}