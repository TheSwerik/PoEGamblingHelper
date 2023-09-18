using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure;

namespace PoEGamblingHelper.Api.Controllers;

public class LeagueController(ILeagueRepository leagueRepository) : ApiControllerBase
{
    [HttpGet] public IAsyncEnumerable<League> GetAll() { return leagueRepository.GetAllLeagues(); }

    [HttpGet("current")]
    [OutputCache(PolicyName = Constants.DataFetcherCacheTag)]
    public League GetCurrent() { return leagueRepository.GetCurrent(); }
}