using PoEGamblingHelper.Api.Extensions;
using PoEGamblingHelper.Application.Repositories;

namespace PoEGamblingHelper.Api.Middleware;

public class AnalyticsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IViewRepository viewRepository)
    {
        var ip = httpContext.Request.GetRealIpAddress();
        if (ip is not null) await viewRepository.AddAsync(ip);

        await next(httpContext);
    }
}

public static class AnalyticsMiddlewareExtensions
{
    public static IApplicationBuilder UseAnalytics(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AnalyticsMiddleware>();
    }
}