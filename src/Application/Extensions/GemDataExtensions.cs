using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Application.Extensions;

public static class GemDataExtensions
{
    public static int LevelModifier(this ResultCase resultCase) { return (int)resultCase - 1; }

    public static decimal AvgProfitPerTry(this GemData gemData,
                                          decimal? rawCost = null,
                                          decimal? worstCaseValue = null,
                                          decimal? middleCaseValue = null,
                                          decimal? bestCaseValue = null,
                                          decimal templeCost = 0)
    {
        var worstCaseProfit = worstCaseValue is null
                                  ? gemData.Profit(ResultCase.Worst, rawCost, templeCost)
                                  : gemData.Profit((decimal)worstCaseValue, rawCost, templeCost);
        var middleCaseProfit = middleCaseValue is null
                                   ? gemData.Profit(ResultCase.Middle, rawCost, templeCost)
                                   : gemData.Profit((decimal)middleCaseValue, rawCost, templeCost);
        var bestCaseProfit = bestCaseValue is null
                                 ? gemData.Profit(ResultCase.Best, rawCost, templeCost)
                                 : gemData.Profit((decimal)bestCaseValue, rawCost, templeCost);
        return (worstCaseProfit + 2 * middleCaseProfit + bestCaseProfit) / 4;
    }

    public static decimal Profit(this GemData gemData,
                                 ResultCase resultCase,
                                 decimal? rawCost = null,
                                 decimal templeCost = 0)
    {
        return gemData.Profit(gemData.Value(resultCase), rawCost, templeCost);
    }

    private static decimal Profit(this GemData gemData,
                                  decimal value,
                                  decimal? rawCost = null,
                                  decimal templeCost = 0)
    {
        return value - gemData.CostPerTry(rawCost, templeCost);
    }

    private static decimal Value(this GemData gemData, ResultCase resultCase)
    {
        return gemData.ResultValue(gemData.MaxLevel() + resultCase.LevelModifier());
    }

    private static decimal ResultValue(this GemData gemData, int level)
    {
        return gemData.Gems.Where(gem => gem.GemLevel == level && gem.Corrupted).MinBy(gem => gem.GemQuality)
                      ?.ChaosValue ?? 0m;
    }

    public static int MaxLevel(this GemData gemData)
    {
        var isAwakened = gemData.Name.ToLowerInvariant().Contains("awakened");
        var isExceptional = gemData.IsExceptional();
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    private static bool IsExceptional(this GemData gemData)
    {
        var lowerName = gemData.Name.ToLowerInvariant();
        return lowerName.Contains("enhance") || lowerName.Contains("empower") || lowerName.Contains("enlighten");
    }

    private static decimal CostPerTry(this GemData gemData,
                                      decimal? rawCost = null,
                                      decimal templeCost = 0)
    {
        return (rawCost ?? gemData.RawCost()) + templeCost;
    }

    public static decimal RawCost(this GemData gemData)
    {
        return gemData.Gems.Where(gem => gem.GemLevel == gemData.MaxLevel() && !gem.Corrupted)
                      .MinBy(gem => gem.GemQuality)?.ChaosValue ?? 0m;
    }

    public static string ToString(this GemData gemData)
    {
        return $"[Id={gemData.Id}, Name={gemData.Name}, MaxLevel={gemData.MaxLevel()}]";
    }
}