using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Backend.Model;

public class Gem : CustomIdEntity
{
    private static readonly List<string> AlternateQualities = new() { "Anomalous", "Divergent", "Phantasmal" };
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GemLevel { get; set; }
    public int GemQuality { get; set; }
    public bool Corrupted { get; set; }

    public int MaxLevel()
    {
        var isAwakened = Name.Contains("Awakened");
        var isExceptional = Name.Contains("Enhance") || Name.Contains("Empower") || Name.Contains("Enlighten");
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    public string TradeQuery(bool accurateLevel = false, bool accurateQuality = false)
    {
        var name = Name;
        var gemAlternateQuality = 0;

        Console.WriteLine(name);
        var firstWord = name.Split(" ")[0];
        if (AlternateQualities.Contains(firstWord))
        {
            name = name[(firstWord.Length + 1)..];
            Console.WriteLine(name);
            gemAlternateQuality = AlternateQualities.IndexOf(firstWord) + 1;
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

    public override string ToString() { return $"[Id={Id}, Name={Name}, MaxLevel={MaxLevel()}]"; }
}