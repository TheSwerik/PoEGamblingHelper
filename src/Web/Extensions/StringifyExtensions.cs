using System.Diagnostics;
using System.Text.RegularExpressions;
using PoEGamblingHelper.Application.QueryParameters;

namespace PoEGamblingHelper.Web.Extensions;

public static partial class StringifyExtensions
{
    public static string Percent(this double value) { return $"{value * 100:0.##}%"; }

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

    public static string ToQueryUrl(this string input) { return QueryUrlRegex().Replace(input, "$1"); }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")] private static partial Regex QueryUrlRegex();
}