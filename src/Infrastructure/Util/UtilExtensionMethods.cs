using Domain.Entity.Gem;
using Infrastructure.Services.FetchDtos;

namespace Infrastructure.Util;

public static class UtilExtensionMethods
{
    public static bool EqualsIgnoreCase(this string? a, string? b)
    {
        return a?.Equals(b, StringComparison.InvariantCultureIgnoreCase)
               ?? b is null;
    }

    public static GemData ToGemData(this IGrouping<string, PoeNinjaGemData> group,
                                    IEnumerable<GemTradeData> gemTradeData)
    {
        return new GemData
               {
                   Name = group.Key,
                   Icon = group.First().Icon,
                   Gems = gemTradeData.Where(tradeData => tradeData.Name.EqualsIgnoreCase(group.Key)).ToList()
               };
    }

    public static GemData ToGemData(this IGrouping<string, PoeNinjaGemData> group,
                                    IEnumerable<GemTradeData> gemTradeData,
                                    IEnumerable<GemData> existingGemData)
    {
        var result = ToGemData(group, gemTradeData);
        result.Id = existingGemData.First(gem => gem.Name.EqualsIgnoreCase(group.Key)).Id;
        return result;
    }
}