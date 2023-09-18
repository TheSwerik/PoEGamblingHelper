using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Repositories;

namespace PoEGamblingHelper.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsDayRepository _analyticsDayRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<AnalyticsService> _logger;
    private readonly IViewRepository _viewRepository;

    public AnalyticsService(IViewRepository viewRepository,
                            ILogger<AnalyticsService> logger,
                            IAnalyticsDayRepository analyticsDayRepository,
                            IDateTimeService dateTimeService)
    {
        _viewRepository = viewRepository;
        _logger = logger;
        _analyticsDayRepository = analyticsDayRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task AddView(string? ipAddress)
    {
        if (ipAddress is not null) await _viewRepository.AddAsync(ipAddress);
    }

    public async Task LogYesterdaysViews()
    {
        var yesterday = _dateTimeService.UtcToday().AddDays(-1);

        var viewCount = await _viewRepository.CountViewsAsync(yesterday);
        _logger.LogInformation("{Views} People used the website on the {YesterdayDay}.{YesterdayMonth}.{YesterdayYear}",
                               viewCount, yesterday.Day, yesterday.Month, yesterday.Year);


        await _analyticsDayRepository.AddAsync(viewCount, yesterday);
        await _viewRepository.RemoveAllAsync(yesterday);
    }
}