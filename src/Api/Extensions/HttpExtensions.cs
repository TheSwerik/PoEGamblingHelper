namespace PoEGamblingHelper.Api.Extensions;

public static class HttpExtensions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var forwardedChain = httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(forwardedChain)) return httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();

        var clientIp = forwardedChain.Split(',').First();
        return clientIp;
    }
}