using Backend.Exceptions;
using Microsoft.AspNetCore.OutputCaching;
using Timer = System.Timers.Timer;

namespace Backend.Service;

public class InitService : IHostedService
{
    private readonly IOutputCacheStore _cache;
    private readonly ILogger<InitService> _logger;
    private readonly IPoeDataFetchService _poeDataFetchService;

    private Timer? _dailyTimer;
    private Timer? _fiveMinuteTimer;

    public InitService(ILogger<InitService> logger, IPoeDataFetchService poeDataFetchService, IOutputCacheStore cache)
    {
        _logger = logger;
        _poeDataFetchService = poeDataFetchService;
        _cache = cache;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start initialization...");

        try
        {
            await _poeDataFetchService.GetCurrentLeague();
            await _poeDataFetchService.GetPriceData();
        }
        catch (PoeGamblingHelperException)
        {
        }

        #region Daily Timer

        _dailyTimer = new Timer(TimeSpan.FromDays(1));
        _dailyTimer.Elapsed += async (_, _) => await _poeDataFetchService.GetCurrentLeague();
        _dailyTimer.AutoReset = true;
        _dailyTimer.Start();

        #endregion

        #region 5 Minute Timer

        _fiveMinuteTimer = new Timer(TimeSpan.FromMinutes(PoeDataFetchService.PoeNinjaFetchMinutes));
        _fiveMinuteTimer.Elapsed += async (_, _) =>
                                    {
                                        await _poeDataFetchService.GetPriceData();
                                        _logger.LogDebug("Cache cleared");
                                        await _cache.EvictByTagAsync("FetchData", new CancellationToken());
                                    };
        _fiveMinuteTimer.AutoReset = true;
        _fiveMinuteTimer.Start();

        #endregion

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fiveMinuteTimer?.Dispose();
        _dailyTimer?.Dispose();
        return Task.CompletedTask;
    }
}