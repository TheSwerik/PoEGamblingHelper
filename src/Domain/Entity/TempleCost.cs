﻿using System.Text.RegularExpressions;
using Domain.Entity.Abstract;

namespace Domain.Entity;

public partial class TempleCost : Entity<Guid>
{
    public DateTime TimeStamp { get; set; } = DateTime.Now.ToUniversalTime();
    public decimal[] ChaosValue { get; set; } = Array.Empty<decimal>();
    public decimal AverageChaosValue() { return ChaosValue.Average(); }

    //TODO move this into frontend or infrastructure
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