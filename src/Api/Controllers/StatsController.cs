using Microsoft.AspNetCore.Mvc;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Stats;

namespace PoEGamblingHelper.Api.Controllers;

public class StatsController(IResultRepository resultRepository) : ApiControllerBase
{
    [HttpGet]
    public IAsyncEnumerable<Result> GetAll()
    {
        return resultRepository.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Result result)
    {
        await resultRepository.SaveAsync(result);
        return Created();
    }
}