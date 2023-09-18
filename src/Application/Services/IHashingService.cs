namespace PoEGamblingHelper.Application.Services;

public interface IHashingService
{
    byte[] HashIpAddress(string ipAddress);
}