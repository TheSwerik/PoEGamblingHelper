using Domain.Exception.Abstract;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Application.Services;

public class InitService : IHostedService
{
    private readonly IOutputCacheStore _cache;
    private readonly string _cacheTag;
    private readonly IDataFetchService _dataFetchService;
    private readonly TimeSpan _fetchInterval;
    private readonly ILogger<InitService> _logger;
    private Timer? _fetchLeagueTimer;
    private Timer? _fetchPriceDataTimer;

    public InitService(ILogger<InitService> logger,
                       IDataFetchService dataFetchService,
                       IOutputCacheStore cache,
                       TimeSpan fetchInterval,
                       string cacheTag)
    {
        _logger = logger;
        _dataFetchService = dataFetchService;
        _cache = cache;
        _fetchInterval = fetchInterval;
        _cacheTag = cacheTag;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fetchPriceDataTimer?.Dispose();
        _fetchLeagueTimer?.Dispose();
        return Task.CompletedTask;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting initialization...");

        await FetchCurrentLeague();
        await FetchPriceData();

        _logger.LogInformation("Initialization finished");

        _logger.LogInformation("Starting fetch timers...");

        _fetchLeagueTimer = new Timer(TimeSpan.FromHours(6));
        _fetchLeagueTimer.Elapsed += async (_, _) => await FetchCurrentLeague();
        _fetchLeagueTimer.AutoReset = true;
        _fetchLeagueTimer.Start();

        _fetchPriceDataTimer = new Timer(_fetchInterval);
        _fetchPriceDataTimer.Elapsed += async (_, _) => await FetchPriceData();
        _fetchPriceDataTimer.AutoReset = true;
        _fetchPriceDataTimer.Start();

        _logger.LogInformation("Fetch timers started");
    }

    private async Task FetchCurrentLeague()
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

    private async Task FetchPriceData()
    {
        await _dataFetchService.FetchCurrencyData();
        await _dataFetchService.FetchTemplePriceData();
        await _dataFetchService.FetchGemPriceData();
        await _cache.EvictByTagAsync(_cacheTag, new CancellationToken());
        _logger.LogDebug("Cache cleared");
    }
}