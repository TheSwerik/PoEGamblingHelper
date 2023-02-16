namespace Api;

public static class ConfigureCache
{
    public static IServiceCollection AddCache(this IServiceCollection services,
                                              IConfiguration configuration)
    {
        var expiration = TimeSpan.FromMinutes(configuration.GetValue<int>("FetchMinutes"));
        var tag = configuration["CacheTag"] ?? throw new Exception("CacheTag is missing");
        return services.AddOutputCache(options =>
                                       {
                                           options.AddBasePolicy(
                                               cacheBuilder => cacheBuilder.Expire(expiration).Tag(tag));
                                       });
    }
}