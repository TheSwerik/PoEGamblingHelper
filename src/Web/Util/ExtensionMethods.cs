using System.Diagnostics;
using System.Net.Http.Json;
using Domain.Exception.Body;
using Domain.QueryParameters;

namespace Web.Util;

public static class ExtensionMethods
{
    public static async Task<PoeGamblingHelperExceptionBody> GetExceptionBody(this HttpContent content)
    {
        return await content.ReadFromJsonAsync<PoeGamblingHelperExceptionBody>() ??
               new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.CannotParseBackendException);
    }

    public static string Round(this decimal value, int places)
    {
        var placeCharacters = new string('#', places);
        return string.Format($"{{0:0.{placeCharacters}}}", value);
    }

    public static string? Round(this decimal? value, int places)
    {
        var placeCharacters = new string('#', places);
        return value is null ? null : string.Format($"{{0:0.{placeCharacters}}}", value);
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
}