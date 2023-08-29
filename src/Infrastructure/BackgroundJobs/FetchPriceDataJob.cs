using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class FetchPriceDataJob : BackgroundJob
{
    private readonly IOutputCacheStore _cache;
    private readonly string _cacheTag = ""; //TODO
    private readonly IDataFetchService _dataFetchService;

    private readonly TimeSpan _interval = TimeSpan.FromHours(6); //TODO
    private readonly ILeagueService _leagueService;
    private readonly ILogger<FetchPriceDataJob> _logger;

    public FetchPriceDataJob(ILogger<FetchPriceDataJob> logger,
                             IDataFetchService dataFetchService,
                             ILeagueService leagueService,
                             IOutputCacheStore cache)
    {
        _logger = logger;
        _dataFetchService = dataFetchService;
        _leagueService = leagueService;
        _cache = cache;
    }

    protected override TimeSpan Interval() { return _interval; }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        League league;
        try
        {
            league = _leagueService.GetCurrentLeague();
        }
        catch (NoLeagueDataException e)
        {
            _logger.LogError("Could not Fetch Price Data, because League could not be get: {Exception}", e);
            return;
        }

        #region Fetch Data

        try
        {
            await _dataFetchService.FetchCurrencyData(league);
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("Could not Fetch CurrencyData: {Exception}", e);
        }

        try
        {
            await _dataFetchService.FetchTemplePriceData(league);
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("Could not Fetch TemplePriceData: {Exception}", e);
        }

        try
        {
            await _dataFetchService.FetchGemPriceData(league);
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("Could not Fetch GemPriceData: {Exception}", e);
        }

        #endregion

        await _cache.EvictByTagAsync(_cacheTag, stoppingToken);
        _logger.LogDebug("Cache cleared");
    }
}