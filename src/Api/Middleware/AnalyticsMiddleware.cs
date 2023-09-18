using PoEGamblingHelper.Api.Extensions;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Api.Middleware;

public class AnalyticsMiddleware
{
    private readonly RequestDelegate _next;

    public AnalyticsMiddleware(RequestDelegate next) { _next = next; }

    public async Task InvokeAsync(HttpContext httpContext, IAnalyticsService analyticsService)
    {
        await analyticsService.AddView(httpContext.Request.GetRealIpAddress());
        await _next(httpContext);
    }
}

public static class AnalyticsMiddlewareExtensions
{
    public static IApplicationBuilder UseAnalytics(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AnalyticsMiddleware>();
    }
}