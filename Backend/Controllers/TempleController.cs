using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Model;

namespace Backend.Controllers;

[ApiController]
[Route("temple")]
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
    [OutputCache(PolicyName = "GetTempleCost")]
    public ActionResult<TempleCost> Get() { return Ok(_templeCostRepository.GetAll().FirstOrDefault()); }
}