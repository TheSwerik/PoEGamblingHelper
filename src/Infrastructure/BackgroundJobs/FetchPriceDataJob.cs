using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Services;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class FetchPriceDataJob : BackgroundJob
{
    private readonly IOutputCacheStore _cache;
    private readonly string _cacheTag = ""; //TODO
    private readonly IDataFetcher _dataFetcher;

    private readonly TimeSpan _interval = TimeSpan.FromHours(6); //TODO
    private readonly ILeagueRepository _leagueRepository;
    private readonly ILogger<FetchPriceDataJob> _logger;

    public FetchPriceDataJob(ILogger<FetchPriceDataJob> logger,
                             IDataFetcher dataFetcher,
                             IOutputCacheStore cache,
                             ILeagueRepository leagueRepository)
    {
        _logger = logger;
        _dataFetcher = dataFetcher;
        _cache = cache;
        _leagueRepository = leagueRepository;
    }

    protected override TimeSpan Interval() { return _interval; }

    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        League league;
        try
        {
            league = _leagueRepository.GetCurrent();
        }
        catch (NoLeagueDataException e)
        {
            _logger.LogError("Could not Fetch Price Data, because League could not be get: {Exception}", e);
            return;
        }

        #region Fetch Data

        try
        {
            await _dataFetcher.FetchCurrencyData(league);
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("Could not Fetch CurrencyData: {Exception}", e);
        }

        try
        {
            await _dataFetcher.FetchTemplePriceData(league);
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("Could not Fetch TemplePriceData: {Exception}", e);
        }

        try
        {
            await _dataFetcher.FetchGemPriceData(league);
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