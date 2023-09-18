using System.Runtime.InteropServices;

namespace PoEGamblingHelper.Api.Extensions;

public static class HttpExtensions
{
    public static string? GetRealIpAddress(this HttpRequest httpRequest)
    {
        var forwardedChain = httpRequest.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(forwardedChain))
        {
            Console.WriteLine("___ MAC ADDERSS ___");
            Console.WriteLine(GetClientMac(httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        var clientIp = forwardedChain.Split(',').First();
        Console.WriteLine(GetClientMac(clientIp));
        return clientIp;
    }

    [DllImport("Iphlpapi.dll")] private static extern int SendARP(int dest, int host, ref long mac, ref int length);

    [DllImport("Ws2_32.dll")] private static extern int inet_addr(string ip);

    private static string GetClientMac(string strClientIp)
    {
        var mac_dest = "";
        try
        {
            var ldest = inet_addr(strClientIp);
            var lhost = inet_addr("");
            var macinfo = new long();
            var len = 6;
            var res = SendARP(ldest, 0, ref macinfo, ref len);
            var mac_src = macinfo.ToString("X");

            while (mac_src.Length < 12) mac_src = mac_src.Insert(0, "0");

            for (var i = 0; i < 11; i++)
                if (0 == i % 2)
                {
                    if (i == 10) mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                    else mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                }
        }
        catch (Exception err)
        {
            throw new Exception("Lỗi " + err.Message);
        }

        return mac_dest;
    }
}