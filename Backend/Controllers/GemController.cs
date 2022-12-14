using Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("data")]
public class GemController : ControllerBase
{
    private readonly IRepository<GemData> _gemRepository;
    private readonly ILogger<GemController> _logger;

    public GemController(ILogger<GemController> logger, IRepository<GemData> gemRepository)
    {
        _logger = logger;
        _gemRepository = gemRepository;
    }

    [HttpGet] public ActionResult<IAsyncEnumerable<GemData>> GetAllGems() { return Ok(_gemRepository.GetAll()); }

    [HttpPost]
    public async Task<ActionResult<GemData>> CreateGem(GemData gem) { return Ok(await _gemRepository.Save(gem)); }
}