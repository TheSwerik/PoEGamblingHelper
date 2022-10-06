using Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("data")]
public class GemController : ControllerBase
{
    private readonly IRepository<Gem> _gemRepository;
    private readonly ILogger<GemController> _logger;

    public GemController(ILogger<GemController> logger, IRepository<Gem> gemRepository)
    {
        _logger = logger;
        _gemRepository = gemRepository;
        _gemRepository.Save(new Gem { Id = 0, Name = "haha" });
        _gemRepository.Save(new Gem { Id = 1, Name = "sdsd" });
    }

    [HttpGet] public ActionResult<IAsyncEnumerable<Gem>> GetAllGems() { return Ok(_gemRepository.GetAll()); }

    [HttpPost] public async Task<ActionResult<Gem>> CreateGem(Gem gem) { return Ok(await _gemRepository.Save(gem)); }
}