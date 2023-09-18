using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsDayRepository _analyticsDayRepository;
    private readonly ILogger<AnalyticsService> _logger;
    private readonly IViewRepository _viewRepository;

    public AnalyticsService(IViewRepository viewRepository,
                            ILogger<AnalyticsService> logger,
                            IAnalyticsDayRepository analyticsDayRepository)
    {
        _viewRepository = viewRepository;
        _logger = logger;
        _analyticsDayRepository = analyticsDayRepository;
    }

    public async Task AddView(string? ipAddress)
    {
        if (ipAddress is null) return;
        var ipHash = SHA512.HashData(Encoding.UTF8.GetBytes(ipAddress));
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var view = new View { IpHash = ipHash, TimeStamp = today };
        await _viewRepository.AddAsync(view);
    }

    public async Task LogYesterdaysViews()
    {
        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);

        var viewCount = await _viewRepository.CountViewsAsync(yesterday);
        _logger.LogInformation("{Views} People used the website on the {YesterdayDay}.{YesterdayMonth}.{YesterdayYear}",
                               viewCount, yesterday.Day, yesterday.Month, yesterday.Year);


        await _analyticsDayRepository.AddAsync(viewCount);
        await _viewRepository.RemoveAllAsync(yesterday);
    }
}