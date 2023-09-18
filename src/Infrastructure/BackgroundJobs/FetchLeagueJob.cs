using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.BackgroundJobs;

public class FetchLeagueJob(ILogger logger,
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