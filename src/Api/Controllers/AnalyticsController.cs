using Microsoft.AspNetCore.Mvc;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Api.Controllers;

public class AnalyticsController(IAnalyticsDayRepository analyticsRepository) : ApiControllerBase
{
    [HttpGet]
    public IAsyncEnumerable<AnalyticsDay> GetAll()
    {
        return analyticsRepository.GetAll();
    }
}