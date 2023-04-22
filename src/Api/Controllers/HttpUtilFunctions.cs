namespace Api.Controllers;

public static class HttpUtilFunctions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var clientIp = httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].First();
        Console.WriteLine(clientIp);
        Console.WriteLine(httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].Count);
        return string.IsNullOrEmpty(clientIp)
                   ? httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString()
                   : clientIp;
    }
}