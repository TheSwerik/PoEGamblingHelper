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
    private readonly IPoeDataService _poeDataService;

    public LeagueController(
        IRepository<League, Guid> leagueRepository,
        IPoeDataService poeDataService
    )
    {
        _leagueRepository = leagueRepository;
        _poeDataService = poeDataService;
    }

    [HttpGet] public IAsyncEnumerable<League> GetAllLeagues() { return _leagueRepository.GetAllAsync(); }

    [HttpGet]
    [Route("current")]
    [OutputCache(PolicyName = "FetchData")]
    public League GetCurrentLeague() { return _poeDataService.GetCurrentLeague() ?? throw new NoLeagueDataException(); }
}