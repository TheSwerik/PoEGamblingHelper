using Timer = System.Timers.Timer;

namespace Backend.Service;

public class ScheduledService : IHostedService
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri("https://poe.ninja/api/data") };

    //https://github.com/5k-mirrors/misc-poe-tools/blob/master/doc/poe-ninja-api.md
    //https://www.pathofexile.com/developer/docs/reference#leagues
    private readonly ILogger<ScheduledService> _logger;
    private string _result = "";

    private Timer? _timer;

    public ScheduledService(ILogger<ScheduledService> logger) { _logger = logger; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(60 * 1000);
        // _timer = new Timer(30 * 60 * 1000);
        _timer.Elapsed += (a, b) =>
                          {
                              var result = _client
                                           .GetStringAsync(
                                               "itemoverview?league=Sentinel&type=SkillGem",
                                               cancellationToken)
                                           .Result;
                              _logger.LogInformation(!result.Equals(_result) ? "{Time}: NEW DATA" : "{Time}: Same",
                                                     DateTime.Now);
                              _result = result;
                          };
        _timer.AutoReset = true;
        Task.Delay(Math.Abs(25 - DateTime.Now.Minute % 30), cancellationToken)
            .ContinueWith((a, b) => _timer.Start(), null, cancellationToken).Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }
}