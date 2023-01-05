using System.Diagnostics;
using Model;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public static class ExtensionMethods
{
    public static string Round(this decimal value, int places)
    {
        var placeCharacters = new string('#', places);
        return string.Format($"{{0:0.{placeCharacters}}}", value);
    }

    public static string Round(this decimal? value, int places)
    {
        var placeCharacters = new string('#', places);
        return string.Format($"{{0:0.{placeCharacters}}}", value);
    }

    public static string ToPrettyString(this Sort sort)
    {
        return sort switch
               {
                   Sort.CostPerTryAsc => "Cost Ascending",
                   Sort.CostPerTryDesc => "Cost Descending",
                   Sort.AverageProfitPerTryAsc => "Average profit per try Ascending",
                   Sort.AverageProfitPerTryDesc => "Average profit per try Descending",
                   Sort.MaxProfitPerTryAsc => "Maximum profit per try Ascending",
                   Sort.MaxProfitPerTryDesc => "Maximum profit per try Descending",
                   _ => throw new UnreachableException(nameof(GemType))
               };
    }

    public static string ToPrettyString(this GemType gemType)
    {
        return gemType switch
               {
                   GemType.All => "All Gems",
                   GemType.Awakened => "Awakened Support Gem",
                   GemType.Exceptional => "Exceptional Support Gem",
                   GemType.Skill => "Skill Gem",
                   GemType.RegularSupport => "Regular Support Gem",
                   _ => throw new UnreachableException(nameof(GemType))
               };
    }

    public static bool ConformsToGemType(this GemData gemData, GemType gemType)
    {
        return gemType switch
               {
                   GemType.Awakened => gemData.Name.StartsWith("Awakened"),
                   GemType.Exceptional => gemData.IsExceptional(),
                   GemType.Skill => !gemData.Name.Contains("Support"),
                   GemType.RegularSupport => gemData.Name.Contains("Support"),
                   _ => true
               };
    }
}