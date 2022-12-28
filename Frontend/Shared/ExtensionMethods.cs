using System.Diagnostics;
using Model;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public static class ExtensionMethods
{
    public static string Round(this decimal value, int places) { return string.Format($"{{0:N{places}}}", value); }

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
                   _ => throw new UnreachableException("Sort")
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