using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Infrastructure.BackgroundJobs;
using PoEGamblingHelper.Infrastructure.Database;
using PoEGamblingHelper.Infrastructure.DataFetcher;
using PoEGamblingHelper.Infrastructure.Repositories;
using PoEGamblingHelper.Infrastructure.Services;

namespace PoEGamblingHelper.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddHttpClient();

        services.AddTransient<ICurrencyRepository, CurrencyRepository>();
        services.AddTransient<IGemRepository, GemRepository>();
        services.AddTransient<ILeagueRepository, LeagueRepository>();
        services.AddTransient<IViewRepository, ViewRepository>();
        services.AddTransient<IAnalyticsDayRepository, AnalyticsDayRepository>();
        services.AddTransient<ITempleRepository, TempleRepository>();

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IHashingService, HashingService>();

        services.AddSingleton<ILeagueDataFetcher, LeagueDataFetcher>();
        services.AddKeyedTransient<IDataFetcher, CurrencyDataFetcher>("currency");
        services.AddKeyedTransient<IDataFetcher, GemDataFetcher>("gem");
        services.AddKeyedTransient<IDataFetcher, TempleDataFetcher>("temple");

        services.AddBackgroundJobs();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DBConnection")
                               + $"Password={configuration["POSTGRES_PASSWORD"]};";
        var assemblyName = typeof(ApplicationDbContext).Assembly.FullName;
        services.AddDbContextFactory<ApplicationDbContext>(opt => opt.UseNpgsql(
                                                               connectionString,
                                                               builder => builder.MigrationsAssembly(assemblyName)
                                                           ));
    }

    private static void AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<FetchLeagueJob>();
        services.AddHostedService<FetchPriceDataJob>();
        services.AddHostedService<LogAnalyticsJob>();
    }
}