using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;

namespace PoEGamblingHelper.Api.Configuration;

public static class ConfigureRateLimiter
{
    public static IServiceCollection AddConfiguredRateLimiter(this IServiceCollection services,
                                                              IConfiguration configuration)
    {
        services.AddRateLimiter(
            limiterOptions =>
            {
                limiterOptions.OnRejected =
                    (context, _) =>
                    {
                        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                            context.HttpContext.Response.Headers.RetryAfter =
                                ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        context.HttpContext
                               .RequestServices
                               .GetService<ILoggerFactory>()?
                               .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                               .LogWarning("OnRejected: {GetUserEndPoint}", GetUserEndPoint(context.HttpContext));
                        return new ValueTask();
                    };

                limiterOptions.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                    PartitionedRateLimiter.Create<HttpContext, string>(_ => GetGlobalRateLimiter(configuration)),
                    PartitionedRateLimiter.Create<HttpContext, IPAddress>(
                        context => GetIpAddressRateLimiter(configuration, context))
                );
            });

        return services;
    }

    private static string GetUserEndPoint(HttpContext context)
    {
        return $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
               + $" {context.Connection.RemoteIpAddress}";
    }

    private static RateLimitPartition<string> GetGlobalRateLimiter(IConfiguration configuration)
    {
        var tokenLimit = configuration.GetValue<int>("RateLimit:Global:TokenLimit");
        var replenishmentPeriod =
            TimeSpan.FromSeconds(configuration.GetValue<int>("RateLimit:Global:ReplenishmentPeriodSeconds"));
        var tokensPerPeriod = configuration.GetValue<int>("RateLimit:Global:TokensPerPeriod");
        var options = new TokenBucketRateLimiterOptions
                      {
                          TokenLimit = tokenLimit,
                          ReplenishmentPeriod = replenishmentPeriod,
                          TokensPerPeriod = tokensPerPeriod,
                          AutoReplenishment = true
                      };

        return RateLimitPartition.GetTokenBucketLimiter("Global", _ => options);
    }

    private static RateLimitPartition<IPAddress> GetIpAddressRateLimiter(
        IConfiguration configuration,
        HttpContext context)
    {
        var remoteIpAddress = context.Connection.RemoteIpAddress;
        if (IPAddress.IsLoopback(remoteIpAddress!)) return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);

        var tokenLimit = configuration.GetValue<int>("RateLimit:IpAddress:TokenLimit");
        var replenishmentPeriod =
            TimeSpan.FromSeconds(configuration.GetValue<int>("RateLimit:IpAddress:ReplenishmentPeriodSeconds"));
        var tokensPerPeriod = configuration.GetValue<int>("RateLimit:IpAddress:TokensPerPeriod");
        var options = new TokenBucketRateLimiterOptions
                      {
                          TokenLimit = tokenLimit,
                          ReplenishmentPeriod = replenishmentPeriod,
                          TokensPerPeriod = tokensPerPeriod,
                          AutoReplenishment = true
                      };

        return RateLimitPartition.GetTokenBucketLimiter(remoteIpAddress!, _ => options);
    }
}