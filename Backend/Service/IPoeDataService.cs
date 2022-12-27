using Model;

namespace Backend.Service;

public interface IPoeDataService
{
    Task<League> GetCurrentLeague();

    Task<string> GetPoeTradeUrl(GemData gem, GemTradeData gemTradeData, bool accurateGemLevel = false,
                                bool accurateGemQuality = false);
}