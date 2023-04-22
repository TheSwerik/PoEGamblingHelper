namespace Api.Controllers;

public static class HttpUtilFunctions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var forwardedChain = httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].First();
        if (string.IsNullOrEmpty(forwardedChain)) return httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        var clientIp = forwardedChain.Split(',').First();
        return clientIp;
    }
}