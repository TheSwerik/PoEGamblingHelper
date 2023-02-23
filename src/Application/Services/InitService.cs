using System.Runtime.CompilerServices;
using Domain.Entity;
using Domain.Exception;
using Domain.Exception.Abstract;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

[assembly: InternalsVisibleTo("Application.Test")]

namespace Application.Services;

public class InitService : IHostedService
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly IOutputCacheStore _cache;
    private readonly string _cacheTag;
    private readonly IDataFetchService _dataFetchService;
    private readonly TimeSpan _fetchInterval;
    private readonly ILeagueService _leagueService;
    private readonly ILogger<InitService> _logger;
    private Timer? _fetchLeagueTimer;
    private Timer? _fetchPriceDataTimer;

    public InitService(ILogger<InitService> logger,
                       IDataFetchService dataFetchService,
                       IOutputCacheStore cache,
                       TimeSpan fetchInterval,
                       string cacheTag,
                       IApplicationDbContextFactory applicationDbContextFactory,
                       ILeagueService leagueService)
    {
        _logger = logger;
        _dataFetchService = dataFetchService;
        _cache = cache;
        _fetchInterval = fetchInterval;
        _cacheTag = cacheTag;
        _leagueService = leagueService;
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _fetchPriceDataTimer?.Dispose();
        _fetchLeagueTimer?.Dispose();
        return Task.CompletedTask;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting initialization...");

        await FetchCurrentLeague();
        await FetchPriceData();

        _logger.LogInformation("Initialization finished");

        _logger.LogInformation("Starting fetch timers...");

        _fetchLeagueTimer = new Timer(TimeSpan.FromHours(6));
        _fetchLeagueTimer.Elapsed += async (_, _) => await FetchCurrentLeague();
        _fetchLeagueTimer.AutoReset = true;
        _fetchLeagueTimer.Start();

        _fetchPriceDataTimer = new Timer(_fetchInterval);
        _fetchPriceDataTimer.Elapsed += async (_, _) => await FetchPriceData();
        _fetchPriceDataTimer.AutoReset = true;
        _fetchPriceDataTimer.Start();

        _logger.LogInformation("Fetch timers started");
    }

    internal async Task FetchCurrentLeague()
    {
        try
        {
            await _dataFetchService.FetchCurrentLeague();
        }
        catch (PoeGamblingHelperException e)
        {
            _logger.LogError("{Exception}", e);
        }
    }

    internal async Task FetchPriceData()
    {
        League league;
        using (var context = _applicationDbContextFactory.CreateDbContext())
        {
            try
            {
                league = _leagueService.GetCurrentLeague(context.League);
            }
            catch (NoLeagueDataException e)
            {
                _logger.LogError("Could not Fetch Price Data, because League could not be get: {Exception}", e);
                return;
            }
        }

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

        await _cache.EvictByTagAsync(_cacheTag, new CancellationToken());
        _logger.LogDebug("Cache cleared");
    }
}