using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class FetchLeagueJob : BackgroundJob
{
    private readonly IDataFetchService _dataFetchService;

    private readonly TimeSpan _interval = TimeSpan.FromHours(6);
    private readonly ILogger<FetchLeagueJob> _logger;

    public FetchLeagueJob(ILogger<FetchLeagueJob> logger, IDataFetchService dataFetchService)
    {
        _logger = logger;
        _dataFetchService = dataFetchService;
    }

    protected override TimeSpan Interval() { return _interval; }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _dataFetchService.FetchCurrentLeague();
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("{Exception}", e);
        }
    }
}