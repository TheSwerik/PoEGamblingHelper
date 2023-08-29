using Microsoft.Extensions.Hosting;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public abstract class BackgroundJob : BackgroundService
{
    private readonly TimeSpan _delay;

    protected BackgroundJob() { _delay = TimeSpan.Zero; }
    protected BackgroundJob(TimeSpan delay) { _delay = delay; }

    protected BackgroundJob(TimeOnly startTime)
    {
        // now 23Uhr, start: 13uhr
        // 13:00 - 23:00 = -10h WRONG -> + 24
        // 13:00 - 03:00  = 10h RIGHT
        _delay = startTime.ToTimeSpan().Subtract(DateTime.UtcNow.TimeOfDay);
        if (_delay < TimeSpan.Zero) _delay = _delay.Add(TimeSpan.FromHours(24));
    }

    protected abstract TimeSpan Interval();
    protected abstract Task ExecuteJobAsync(CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(_delay, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExecuteJobAsync(stoppingToken);
            await Task.Delay(Interval(), stoppingToken);
        }
    }
}