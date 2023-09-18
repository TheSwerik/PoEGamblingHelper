using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public abstract class BackgroundJob : BackgroundService
{
    private readonly TimeSpan _delay;
    private readonly TimeSpan _interval;

    /// <summary>
    /// Gets the StartTime from the configuration as well.
    /// </summary>
    protected BackgroundJob(IConfiguration configuration, IDateTimeService dateTimeService) : this(configuration)
    {
        //TODO write unit tests
        // start: 13uhr, now: 23Uhr
        // 13:00 - 03:00  = 10h RIGHT
        // 13:00 - 23:00 = -10h WRONG -> + 24 = 14
        var startTime = configuration.GetSection("BackgroundJobIntervals")
                                     .GetValue<TimeOnly>(GetType().Name + "StartTime");
        _delay = startTime.ToTimeSpan().Subtract(dateTimeService.UtcNow().TimeOfDay);
        if (_delay < TimeSpan.Zero) _delay = _delay.Add(TimeSpan.FromHours(24));
    }

    protected BackgroundJob(IConfiguration configuration) : this(TimeSpan.Zero, configuration) { }

    private BackgroundJob(TimeSpan delay, IConfiguration configuration)
    {
        _delay = delay;
        _interval = configuration.GetSection("BackgroundJobIntervals")
                                 .GetValue<TimeSpan>(GetType().Name);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(_delay, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExecuteJobAsync(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    protected abstract Task ExecuteJobAsync(CancellationToken stoppingToken);
}