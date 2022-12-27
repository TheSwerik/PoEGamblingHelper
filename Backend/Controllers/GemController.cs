using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Backend.Controllers;

[ApiController]
[Route("data")]
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
    [OutputCache(PolicyName = "GetAllGems")]
    public ActionResult<IAsyncEnumerable<GemData>> GetAllGems()
    {
        return Ok(_gemDataRepository.GetAllAsync(dbset => dbset.Include(gemData => gemData.Gems).AsAsyncEnumerable()));
    }

    [HttpPost]
    public async Task<ActionResult<GemData>> CreateGem(GemData gem) { return Ok(await _gemDataRepository.Save(gem)); }
}