namespace Api.Controllers;

public static class HttpUtilFunctions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        Console.WriteLine(httpRequest.HttpContext.Request.Headers["X-Forwarded-For"]);
        var clientIp = httpRequest.HttpContext.Request.Headers["X-Real-IP"];
        Console.WriteLine(clientIp);
        if (string.IsNullOrEmpty(clientIp)) clientIp = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        Console.WriteLine(clientIp);
        return clientIp;
    }
}