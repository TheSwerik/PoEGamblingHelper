using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GemController : ControllerBase
{
    private readonly IRepository<GemData, Guid> _gemDataRepository;
    private readonly ILogger<GemController> _logger;

    public GemController(ILogger<GemController> logger, IRepository<GemData, Guid> gemDataRepository)
    {
        _logger = logger;
        _gemDataRepository = gemDataRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public ActionResult<IAsyncEnumerable<GemData>> GetAll([FromQuery] Page? page)
    {
        return Ok(_gemDataRepository.GetAllAsync(page, dbset => dbset.Include(gemData => gemData.Gems)));
    }
}