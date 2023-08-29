using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class LogAnalyticsJob : BackgroundJob
{
    private readonly IAnalyticsService _analyticsService;

    private readonly TimeSpan _interval = TimeSpan.FromDays(1);

    public LogAnalyticsJob(IAnalyticsService analyticsService) { _analyticsService = analyticsService; }

    protected override TimeSpan Interval() { return _interval; }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        await _analyticsService.LogYesterdaysViews();
    }
}