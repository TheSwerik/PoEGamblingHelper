using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Model;

public class GemTradeData : CustomIdEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }
    public string DetailsId { get; set; }
    public decimal ChaosValue { get; set; }
    public decimal ExaltedValue { get; set; }
    public decimal DivineValue { get; set; }
    public int ListingCount { get; set; }

    public string TradeQuery(string name, bool accurateLevel = false, bool accurateQuality = false)
    {
        var gemAlternateQuality = 0;

        Console.WriteLine(name);
        var firstWord = name.Split(" ")[0];

        if (Enum.TryParse(firstWord, true, out AlternateQuality quality))
        {
            name = name[(firstWord.Length + 1)..];
            Console.WriteLine(name);
            gemAlternateQuality = (int)quality + 1;
        }

        var minGemLevel = accurateLevel ? GemLevel : int.MinValue;
        var maxGemLevel = accurateLevel ? GemLevel : int.MaxValue;

        var minGemQuality = accurateQuality ? GemQuality : int.MinValue;
        var maxGemQuality = accurateQuality ? GemQuality : int.MaxValue;

        // var minGemLevel = accurateLevel ? GemLevel : 0;
        // var maxGemLevel = accurateLevel ? GemLevel : MaxLevel();

        // var minGemQuality = accurateQuality ? GemQuality : 0;
        // var maxGemQuality = accurateQuality ? GemQuality : 23;

        return $@"
            {{
              ""query"": {{
                ""filters"": {{
                  ""misc_filters"": {{
                    ""filters"": {{
                      ""gem_level"": {{
                        ""min"": {minGemLevel},
                        ""max"": {maxGemLevel}
                      }},
                      ""gem_alternate_quality"": {{
                        ""option"": ""{gemAlternateQuality}""
                      }},
                      ""quality"": {{
                        ""min"": {minGemQuality},
                        ""max"": {maxGemQuality}
                      }}
                    }}
                  }}
                }},
                ""type"": ""{HttpUtility.UrlPathEncode(name)}""
              }}
            }}
        ";
    }
}