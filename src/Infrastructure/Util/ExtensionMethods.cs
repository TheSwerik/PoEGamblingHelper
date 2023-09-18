using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure.Services.FetchDtos;

namespace PoEGamblingHelper.Infrastructure.Util;

public static class ExtensionMethods
{
    public static GemData ToGemData(this IGrouping<string, PoeNinjaGemData> group,
                                    IEnumerable<GemTradeData> gemTradeData,
                                    IEnumerable<GemData> existingGemData)
    {
        var result = ToGemData(group, gemTradeData);
        result.Id = existingGemData.First(gem => gem.Name.EqualsIgnoreCase(group.Key)).Id;
        return result;
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
}