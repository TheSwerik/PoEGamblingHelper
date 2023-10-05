using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class FetchPriceDataJob(ILogger<FetchPriceDataJob> logger,
                               IOutputCacheStore cache,
                               ILeagueRepository leagueRepository,
                               IConfiguration configuration,
                               [FromKeyedServices("currency")] IDataFetcher currencyDataFetcher,
                               [FromKeyedServices("temple")] IDataFetcher templeDataFetcher,
                               [FromKeyedServices("gem")] IDataFetcher gemDataFetcher)
    : BackgroundJob(configuration)
{
    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        League league;
        try
        {
            league = leagueRepository.GetCurrent();
        }
        catch (NoLeagueDataException e)
        {
            logger.LogError("Could not Fetch Price Data, because League could not be get: {Exception}", e);
            return;
        }

        #region Fetch Data

        try
        {
            await currencyDataFetcher.Fetch(league);
        }
        catch (PoeGamblingHelperException e)
        {
            logger.LogError("Could not Fetch CurrencyData: {Exception}", e);
        }

        try
        {
            await templeDataFetcher.Fetch(league);
        }
        catch (PoeGamblingHelperException e)
        {
            logger.LogError("Could not Fetch TemplePriceData: {Exception}", e);
        }

        try
        {
            await gemDataFetcher.Fetch(league);
        }
        catch (PoeGamblingHelperException e)
        {
            logger.LogError("Could not Fetch GemPriceData: {Exception}", e);
        }

        #endregion

        await cache.EvictByTagAsync(Constants.DataFetcherCacheTag, stoppingToken);
        logger.LogDebug("Cache cleared");
    }
}