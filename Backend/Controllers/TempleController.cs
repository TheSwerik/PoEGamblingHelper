using Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Shared.Entity;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TempleController : ControllerBase
{
    private readonly ILogger<TempleController> _logger;
    private readonly IRepository<TempleCost, Guid> _templeCostRepository;

    public TempleController(ILogger<TempleController> logger, IRepository<TempleCost, Guid> templeCostRepository)
    {
        _logger = logger;
        _templeCostRepository = templeCostRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public ActionResult<TempleCost> Get()
    {
        return Ok(_templeCostRepository.GetAll().FirstOrDefault() ?? throw new NoTempleDataException());
    }
}