﻿using Timer = System.Timers.Timer;

namespace Backend.Service;

public class InitService : IHostedService
{
    private const int Second = 1000;
    private const int Minute = 60 * Second;
    private const int Hour = 60 * Minute;
    private const int Day = 24 * Hour;

    private readonly ILogger<InitService> _logger;
    private readonly IPoeDataFetchService _poeDataFetchService;

    private Timer? _dailyTimer;
    private Timer? _fiveMinuteTimer;

    public InitService(ILogger<InitService> logger, IPoeDataFetchService poeDataFetchService)
    {
        _logger = logger;
        _poeDataFetchService = poeDataFetchService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start initialization...");

        await _poeDataFetchService.GetCurrentLeague();
        await _poeDataFetchService.GetPriceData();

        #region Daily Timer

        _dailyTimer = new Timer(Day);
        _dailyTimer.Elapsed += async (_, _) => await _poeDataFetchService.GetCurrentLeague();
        _dailyTimer.AutoReset = true;
        _dailyTimer.Start();

        #endregion

        #region 5 Minute Timer

        _fiveMinuteTimer = new Timer(5 * Minute);
        _fiveMinuteTimer.Elapsed += async (_, _) => await _poeDataFetchService.GetPriceData();
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