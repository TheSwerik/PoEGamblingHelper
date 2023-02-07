using Backend.Exceptions;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Shared.Entity;

namespace Backend.Controllers;

[ApiController]
[Route("league")]
public class LeagueController : ControllerBase
{
    private readonly IRepository<League, Guid> _leagueRepository;
    private readonly ILogger<LeagueController> _logger;
    private readonly IPoeDataService _poeDataService;

    public LeagueController(
        ILogger<LeagueController> logger,
        IRepository<League, Guid> leagueRepository,
        IPoeDataService poeDataService
    )
    {
        _logger = logger;
        _leagueRepository = leagueRepository;
        _poeDataService = poeDataService;
    }

    [HttpGet] public ActionResult<IAsyncEnumerable<League>> GetAllLeagues() { return Ok(_leagueRepository.GetAll()); }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public ActionResult<League> GetCurrentLeague()
    {
        return Ok(_poeDataService.GetCurrentLeague() ?? throw new NoLeagueDataException());
    }
}