using Timer = System.Timers.Timer;

namespace Backend.Service;

public class InitService : IHostedService
{
    private readonly ILogger<InitService> _logger;
    private readonly PoEDataService _poeDataService;

    private Timer? _priceDataTimer;

    public InitService(ILogger<InitService> logger, PoEDataService poeDataService)
    {
        _logger = logger;
        _poeDataService = poeDataService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _poeDataService.GetCurrentLeague();

        #region 5 Minute Timer

        _priceDataTimer = new Timer(5 * 60 * 1000);
        _priceDataTimer.Elapsed += async (_, _) => await _poeDataService.GetPriceData();
        _priceDataTimer.AutoReset = true;
        _priceDataTimer.Start();

        #endregion
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _priceDataTimer?.Dispose();
        return Task.CompletedTask;
    }
}