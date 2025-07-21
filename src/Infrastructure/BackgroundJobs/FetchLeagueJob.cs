using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class FetchLeagueJob(ILogger<FetchLeagueJob> logger,
                            ILeagueDataFetcher leagueDataFetcher,
                            IConfiguration configuration)
    : BackgroundJob(configuration)
{
    protected override async Task ExecuteJobAsync(CancellationToken stoppingToken)
    {
        try
        {
            await leagueDataFetcher.Fetch();
        }
        catch (PoeGamblingHelperException e)
        {
            logger.LogError("{Exception}", e);
        }
    }
}