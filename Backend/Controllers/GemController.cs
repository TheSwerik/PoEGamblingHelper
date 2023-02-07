using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Model;
using Model.QueryParameters;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GemController : ControllerBase
{
    private readonly IGemDataRepository _gemDataRepository;
    private readonly ILogger<GemController> _logger;

    public GemController(ILogger<GemController> logger, IGemDataRepository gemDataRepository)
    {
        _logger = logger;
        _gemDataRepository = gemDataRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public ActionResult<IEnumerable<GemData>> GetAll([FromQuery] GemDataQuery? query, [FromQuery] Page? page)
    {
        return Ok(_gemDataRepository.GetAll(query, page));
    }
}