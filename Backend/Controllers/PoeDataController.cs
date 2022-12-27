using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Backend.Controllers;

[ApiController]
[Route("poe")]
public class PoeDataController : ControllerBase
{
    private readonly IRepository<League> _leagueRepository;
    private readonly ILogger<PoeDataController> _logger;
    private readonly IPoeDataService _poeDataService;

    public PoeDataController(
        ILogger<PoeDataController> logger,
        IRepository<League> leagueRepository,
        IPoeDataService poeDataService
    )
    {
        _logger = logger;
        _leagueRepository = leagueRepository;
        _poeDataService = poeDataService;
    }

    [HttpGet]
    [Route("league")]
    public ActionResult<IAsyncEnumerable<League>> GetAllLeagues() { return Ok(_leagueRepository.GetAll()); }

    [HttpGet]
    [Route("league/current")]
    public ActionResult<League> GetCurrentLeague() { return Ok(_poeDataService.GetCurrentLeague()); }
}