using System.Text.RegularExpressions;

namespace Model;

public partial class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public decimal[] ChaosValue { get; set; }
    public decimal AverageChaosValue() { return ChaosValue.Average(); }

    public static string TradeUrl(League currentLeague)
    {
        const string poeTradeUrl = "https://www.pathofexile.com/trade/search";
        const string queryKey = "?q=";

        var query = JsonMinifyRegex().Replace(@"
        {
          ""query"":{
            ""stats"":[
              {
                ""type"":""and"",
                ""filters"":[
                  {
                    ""id"":""pseudo.pseudo_temple_gem_room_3"",
                    ""value"":{
                      ""option"":1
                    },
                    ""disabled"":false
                  }
                ]
              }
            ],
            ""type"": ""Chronicle of Atzoatl""
          }
        }
        ", "$1");
        return $"{poeTradeUrl}/{currentLeague.Name}{queryKey}{query}";
    }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")] private static partial Regex JsonMinifyRegex();
}