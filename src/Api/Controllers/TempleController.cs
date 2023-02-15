using Domain.Entity;
using Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class TempleController : ApiControllerBase
{
    private readonly ITempleService _templeService;

    public TempleController(ITempleService templeService) { _templeService = templeService; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public TempleCost Get() { return _templeService.GetCurrent() ?? throw new NoTempleDataException(); }
}