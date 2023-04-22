namespace Api.Controllers;

public static class HttpUtilFunctions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var clientIp = httpRequest.HttpContext.Request.Headers["X-Real-IP"];
        if (string.IsNullOrEmpty(clientIp)) clientIp = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        return clientIp;
    }
}