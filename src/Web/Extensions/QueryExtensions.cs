using System.Web;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Web.Extensions;

public static class QueryExtensions
{
    public static string ToQueryString(this GemDataQuery gemDataQuery, PageRequest? page)
    {
        return page is null
                   ? gemDataQuery.ToQueryString()
                   : $"{page.ToQueryString()}{gemDataQuery.ToQueryString(false)}";
    }

    public static string ToQueryString(this GemDataQuery gemDataQuery, bool questionMark = true)
    {
        var start = questionMark ? "?" : "&";
        var searchText = gemDataQuery.SearchText == string.Empty ? "" : $"&searchText={gemDataQuery.SearchText}";
        var pricePerTryFrom = gemDataQuery.PricePerTryFrom is null
                                  ? ""
                                  : $"&pricePerTryFrom={gemDataQuery.PricePerTryFrom}";
        var pricePerTryTo = gemDataQuery.PricePerTryTo is null ? "" : $"&pricePerTryTo={gemDataQuery.PricePerTryTo}";
        return
            $"{start}sort={gemDataQuery.Sort}&gemType={gemDataQuery.GemType}&onlyShowProfitable={gemDataQuery.OnlyShowProfitable}&showVaal={gemDataQuery.ShowVaal}{searchText}{pricePerTryFrom}{pricePerTryTo}";
    }

    public static string ToQueryString(this PageRequest pageRequest)
    {
        return $"?pageNumber={pageRequest.PageNumber}&pageSize={pageRequest.PageSize}";
    }

    public static string TradeUrl(this GemTradeData gemTradeData,
                                  string currentLeague,
                                  bool accurateLevel = true,
                                  bool accurateQuality = false)
    {
        const string poeTradeUrl = "https://www.pathofexile.com/trade/search";
        const string queryKey = "?q=";
        return $"{poeTradeUrl}/{currentLeague}{queryKey}{gemTradeData.TradeQuery(accurateLevel, accurateQuality)}";
    }

    public static string TradeQuery(this GemTradeData gemTradeData,
                                    bool accurateLevel = true,
                                    bool accurateQuality = false)
    {
        var gemAlternateQuality = -1;

        var firstWord = gemTradeData.Name.Split(" ")[0];

        var name = gemTradeData.Name;
        if (Enum.TryParse(firstWord, true, out AlternateQuality quality))
        {
            name = gemTradeData.Name[(firstWord.Length + 1)..];
            gemAlternateQuality = (int)quality + 1;
        }

        var corruptedText = $"""
                             "corrupted": {gemTradeData.Corrupted.ToString().ToLower()}
                             """;

        var minGemLevel = accurateLevel ? gemTradeData.GemLevel : int.MinValue;
        var maxGemLevel = accurateLevel ? gemTradeData.GemLevel : int.MaxValue;
        var levelText = !accurateLevel
                            ? string.Empty
                            : $$""","gem_level": {"min": {{minGemLevel}},"max": {{maxGemLevel}}}""";

        var minGemQuality = accurateQuality ? gemTradeData.GemQuality : int.MinValue;
        var maxGemQuality = accurateQuality ? gemTradeData.GemQuality : int.MaxValue;
        var qualityText = !accurateQuality
                              ? string.Empty
                              : $$""","quality": {"min": {{minGemQuality}},"max": {{maxGemQuality}}}""";

        var gemAlternateQualityText = gemAlternateQuality < 0
                                          ? string.Empty
                                          : $$""","gem_alternate_quality": {"option": "{{gemAlternateQuality}}"},""";


        return $$"""
                 {
                    "query": {
                      "filters": {
                          "misc_filters": {
                              "filters": {
                                  {{corruptedText}}
                                  {{levelText}}
                                  {{gemAlternateQualityText}}
                                  {{qualityText}}
                              }
                          }
                      },
                      "type": "{{HttpUtility.UrlPathEncode(name)}}"
                    }
                 }
                 """.ToQueryUrl();
    }
}