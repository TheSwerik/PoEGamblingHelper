namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public static class PoeToolUrls
{
    public const string PoeApiUrl = "https://www.pathofexile.com/api";
    public const string PoeDbUrl = "https://poedb.tw/us";
    public const string PoeNinjaUrl = "https://poe.ninja/api/data";
    public static string PoeDbLeagueUrl => $"{PoeDbUrl}/League#LeaguesList";
    public static string PoeNinjaCurrencyUrl => $"{PoeNinjaUrl}/currencyoverview?type=Currency";
    public static string PoeNinjaGemUrl => $"{PoeNinjaUrl}/itemoverview?type=SkillGem";
    public static string PoeApiTradeUrl => $"{PoeApiUrl}/trade";
}