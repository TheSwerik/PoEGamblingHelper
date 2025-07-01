using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class LogAnalyticsJob(
    IDateTimeService dateTimeService,
    IViewRepository viewRepository,
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    ILogger<LogAnalyticsJob> logger,
    IAnalyticsDayRepository analyticsDayRepository,
    IConfiguration configuration)
    : BackgroundJob(configuration, dateTimeService)
{
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        var yesterday = _dateTimeService.UtcToday().AddDays(-1);

        var views = await viewRepository.CountAsync(yesterday);
        logger.LogInformation("{Views} People used the website on the {YesterdayDay}.{YesterdayMonth}.{YesterdayYear}",
                              views,
                              yesterday.Day,
                              yesterday.Month,
                              yesterday.Year);

        await analyticsDayRepository.AddAsync(views, yesterday);
        await viewRepository.RemoveAllAsync(yesterday);
    }
}