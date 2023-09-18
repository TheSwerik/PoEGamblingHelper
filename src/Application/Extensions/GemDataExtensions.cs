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
        var worstCaseProfit = gemData.Profit(worstCaseValue ?? gemData.Value(ResultCase.Worst), rawCost, templeCost);
        var middleCaseProfit = gemData.Profit(middleCaseValue ?? gemData.Value(ResultCase.Middle), rawCost, templeCost);
        var bestCaseProfit = gemData.Profit(bestCaseValue ?? gemData.Value(ResultCase.Best), rawCost, templeCost);

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
                                  decimal resultValue,
                                  decimal? rawCost = null,
                                  decimal templeCost = 0)
    {
        return resultValue - gemData.CostPerTry(rawCost, templeCost);
    }

    /// <summary>
    ///     Finds the ChaosValue of the Gem for the specified resultCase. <see cref="ResultValue" />
    ///     If there are multiple, it just gets the cheapest one.
    /// </summary>
    private static decimal Value(this GemData gemData, ResultCase resultCase)
    {
        return gemData.ResultValue(gemData.MaxLevel() + resultCase.LevelModifier());
    }

    /// <summary>
    ///     Finds the ChaosValue of the Gem with the specified level and corrupted.
    ///     If there are multiple, it just gets the cheapest one.
    /// </summary>
    private static decimal ResultValue(this GemData gemData, int level)
    {
        return gemData.Gems
                      .Where(gem => gem.GemLevel == level && gem.Corrupted)
                      .MinBy(gem => gem.ChaosValue)
                      ?.ChaosValue ?? 0m;
    }

    public static int MaxLevel(this GemData gemData)
    { //TODO this is incorrect, I need to pull this data from somewhere
        var lowerName = gemData.Name.ToLowerInvariant();
        var isAwakened = lowerName.Contains("awakened");
        var isExceptional = lowerName.Contains("enhance")
                            || lowerName.Contains("empower")
                            || lowerName.Contains("enlighten");
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    private static decimal CostPerTry(this GemData gemData, decimal? rawCost = null, decimal templeCost = 0)
    {
        return (rawCost ?? gemData.RawCost()) + templeCost;
    }

    /// <summary>
    ///     Finds the ChaosValue of the Gem with maximum level and not corrupted.
    ///     If there are multiple, it just gets the cheapest one.
    /// </summary>
    public static decimal RawCost(this GemData gemData)
    {
        return gemData.Gems
                      .Where(gem => gem.GemLevel == gemData.MaxLevel() && !gem.Corrupted)
                      .MinBy(gem => gem.ChaosValue)?
                      .ChaosValue ?? 0m;
    }
}