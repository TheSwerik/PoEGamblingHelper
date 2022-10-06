using Timer = System.Timers.Timer;

namespace Backend.Service;

public class InitService : IHostedService
{
    private readonly ILogger<InitService> _logger;
    private readonly IPoEDataService _poeDataService;

    private Timer? _priceDataTimer;

    public InitService(ILogger<InitService> logger, IPoEDataService poeDataService)
    {
        _logger = logger;
        _poeDataService = poeDataService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start initialization...");

        await _poeDataService.GetCurrentLeague();

        #region 5 Minute Timer

        await _poeDataService.GetPriceData();
        _priceDataTimer = new Timer(5 * 60 * 1000);
        _priceDataTimer.Elapsed += async (_, _) => await _poeDataService.GetPriceData();
        _priceDataTimer.AutoReset = true;
        _priceDataTimer.Start();

        #endregion

        _logger.LogInformation("Initialization finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _priceDataTimer?.Dispose();
        return Task.CompletedTask;
    }
}