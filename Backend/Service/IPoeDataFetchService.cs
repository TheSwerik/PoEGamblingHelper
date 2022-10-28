using Backend.Model;

namespace Backend.Service;

public interface IPoeDataFetchService : IDisposable
{
    Task GetCurrentLeague();
    Task GetPriceData();
    Task<string> GetPoeTradeUrl(Gem gem, bool accurateGemLevel = false, bool accurateGemQuality = false);
}