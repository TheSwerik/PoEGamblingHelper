using System.Security.Cryptography;
using System.Text;
using PoEGamblingHelper.Application.Services;

namespace PoEGamblingHelper.Infrastructure.Services;

public class HashingService : IHashingService
{
    public byte[] HashIpAddress(string ipAddress) { return SHA512.HashData(Encoding.UTF8.GetBytes(ipAddress)); }
}