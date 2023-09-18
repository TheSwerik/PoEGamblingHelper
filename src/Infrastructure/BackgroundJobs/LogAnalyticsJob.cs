using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class LogAnalyticsJob : BackgroundJob
{
    private readonly IAnalyticsDayRepository _analyticsDayRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<LogAnalyticsJob> _logger;
    private readonly IViewRepository _viewRepository;

    public LogAnalyticsJob(IDateTimeService dateTimeService,
                           IViewRepository viewRepository,
                           ILogger<LogAnalyticsJob> logger,
                           IAnalyticsDayRepository analyticsDayRepository,
                           IConfiguration configuration)
        : base(configuration, dateTimeService)
    {
        _dateTimeService = dateTimeService;
        _viewRepository = viewRepository;
        _logger = logger;
        _analyticsDayRepository = analyticsDayRepository;
    }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        var yesterday = _dateTimeService.UtcToday().AddDays(-1);

        var views = await _viewRepository.CountViewsAsync(yesterday);
        _logger.LogInformation("{Views} People used the website on the {YesterdayDay}.{YesterdayMonth}.{YesterdayYear}",
                               views, yesterday.Day, yesterday.Month, yesterday.Year);

        await _analyticsDayRepository.AddAsync(views, yesterday);
        await _viewRepository.RemoveAllAsync(yesterday);
    }
}