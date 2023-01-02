using System.Text.RegularExpressions;
using System.Web;

namespace Model;

public partial class GemTradeData : Entity<long>
{
    public string Name { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }
    public string DetailsId { get; set; }
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }

    public string TradeUrl(League currentLeague, bool accurateLevel = true, bool accurateQuality = false)
    {
        const string poeTradeUrl = "https://www.pathofexile.com/trade/search";
        const string queryKey = "?q=";
        return $"{poeTradeUrl}/{currentLeague.Name}{queryKey}{TradeQuery(accurateLevel, accurateQuality)}";
    }

    public string TradeQuery(bool accurateLevel = true, bool accurateQuality = false)
    {
        var gemAlternateQuality = -1;

        var firstWord = Name.Split(" ")[0];

        if (Enum.TryParse(firstWord, true, out AlternateQuality quality))
        {
            Name = Name[(firstWord.Length + 1)..];
            gemAlternateQuality = (int)quality + 1;
        }

        var corruptedText = $@"""corrupted"": {Corrupted.ToString().ToLower()}";

        var minGemLevel = accurateLevel ? GemLevel : int.MinValue;
        var maxGemLevel = accurateLevel ? GemLevel : int.MaxValue;
        var levelText = !accurateLevel
                            ? string.Empty
                            : $@",""gem_level"": {{""min"": {minGemLevel},""max"": {maxGemLevel}}}";

        var minGemQuality = accurateQuality ? GemQuality : int.MinValue;
        var maxGemQuality = accurateQuality ? GemQuality : int.MaxValue;
        var qualityText = !accurateQuality
                              ? string.Empty
                              : $@",""quality"": {{""min"": {minGemQuality},""max"": {maxGemQuality}}}";

        var gemAlternateQualityText = gemAlternateQuality < 0
                                          ? string.Empty
                                          : $@",""gem_alternate_quality"": {{""option"": ""{gemAlternateQuality}""}},";

        return JsonMinifyRegex().Replace($@"
            {{
              ""query"": {{
                ""filters"": {{
                  ""misc_filters"": {{
                    ""filters"": {{
                      {corruptedText}
                      {levelText}
                      {gemAlternateQualityText}
                      {qualityText}
                    }}
                  }}
                }},
                ""type"": ""{HttpUtility.UrlPathEncode(Name)}""
              }}
            }}
        ", "$1");
    }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")] private static partial Regex JsonMinifyRegex();
}