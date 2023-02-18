namespace Api;

public static class ConfigureCache
{
    public static IServiceCollection AddCache(this IServiceCollection services,
                                              IConfiguration configuration)
    {
        var expiration = TimeSpan.FromMinutes(configuration.GetValue<int>("FetchInterval"));
        var tag = configuration.GetValue<string>("CacheTag")!;
        return services.AddOutputCache(options =>
                                       {
                                           options.AddBasePolicy(
                                               cacheBuilder => cacheBuilder.Expire(expiration).Tag(tag));
                                       });
    }
}