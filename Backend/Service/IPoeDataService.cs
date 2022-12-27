using Model;

namespace Backend.Service;

public interface IPoeDataService
{
    Task<League> GetCurrentLeague();
    Task<string> GetPoeTradeUrl(Gem gem, bool accurateGemLevel = false, bool accurateGemQuality = false);
}