using Microsoft.AspNetCore.Mvc;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Api.Controllers;

public class AnalyticsController(IAnalyticsDayRepository analyticsRepository) : ApiControllerBase
{
    [HttpGet]
    public IAsyncEnumerable<AnalyticsDay> Get([FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
    {
        if (start is null || end is null) return analyticsRepository.GetAll();

        return analyticsRepository.Get(start.Value, end.Value);
    }
}