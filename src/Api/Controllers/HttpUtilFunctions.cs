namespace Api.Controllers;

public static class HttpUtilFunctions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var forwardedChain = httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].First();
        Console.WriteLine(forwardedChain);
        if (string.IsNullOrEmpty(forwardedChain)) return httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        var clientIp = forwardedChain.Split(',').First();
        Console.WriteLine(clientIp);
        return clientIp;
    }
}