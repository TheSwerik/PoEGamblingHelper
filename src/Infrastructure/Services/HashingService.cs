using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.Services;

[SuppressMessage("Performance", "CA1822:Member als statisch markieren")]
public class HashingService : IHashingService
{
    public byte[] HashIpAddress(string ipAddress) { return SHA512.HashData(Encoding.UTF8.GetBytes(ipAddress)); }
}