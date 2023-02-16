using Application.Services;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services,
                                                 IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("PoEGamblingHelper"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DBConnection")
                                   + $"Password={configuration["POSTGRES_PASSWORD"]};";
            var assemblyName = typeof(ApplicationDbContext).Assembly.FullName;
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(
                                                            connectionString,
                                                            builder => builder.MigrationsAssembly(assemblyName)
                                                        ));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IGemService, GemService>();
        services.AddTransient<ILeagueService, LeagueService>();
        services.AddTransient<IDataFetchService, DataFetchService>();
    }
}