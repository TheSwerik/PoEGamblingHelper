using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Infrastructure.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class FetchLeagueJob : BackgroundJob
{
    private readonly IDataFetcher _dataFetcher;

    private readonly TimeSpan _interval = TimeSpan.FromHours(6);
    private readonly ILogger<FetchLeagueJob> _logger;

    public FetchLeagueJob(ILogger<FetchLeagueJob> logger, IDataFetcher dataFetcher)
    {
        _logger = logger;
        _dataFetcher = dataFetcher;
    }

    protected override TimeSpan Interval() { return _interval; }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _dataFetcher.FetchCurrentLeague();
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("{Exception}", e);
        }
    }
}