using Shared.Util;

namespace Shared.Entity;

public class GemData : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public ICollection<GemTradeData> Gems { get; set; } = new List<GemTradeData>();

    public int MaxLevel()
    {
        var isAwakened = Name.Contains("Awakened");
        var isExceptional = IsExceptional();
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    public override string ToString() { return $"[Id={Id}, Name={Name}, MaxLevel={MaxLevel()}]"; }

    public decimal RawCost()
    {
        return Gems.Where(gem => gem.GemLevel == MaxLevel() && !gem.Corrupted)
                   .MinBy(gem => gem.GemQuality)?.ChaosValue ?? 0m;
    }

    public decimal Value(ResultCase resultCase) { return ResultValue(MaxLevel() + resultCase.LevelModifier()); }
    public decimal CostPerTry(decimal? rawCost, decimal templeCost = 0) { return (rawCost ?? RawCost()) + templeCost; }

    public decimal Profit(decimal value, decimal? rawCost = null, decimal templeCost = 0)
    {
        return value - CostPerTry(rawCost, templeCost);
    }

    public decimal Profit(ResultCase resultCase, decimal? rawCost = null, decimal templeCost = 0)
    {
        return Profit(Value(resultCase), rawCost, templeCost);
    }

    public decimal AvgProfitPerTry(decimal? rawCost = null,
                                   decimal? worstCaseValue = null,
                                   decimal? middleCaseValue = null,
                                   decimal? bestCaseValue = null,
                                   decimal templeCost = 0)
    {
        var worstCaseProfit = worstCaseValue is null
                                  ? Profit(ResultCase.Worst, rawCost, templeCost)
                                  : Profit((decimal)worstCaseValue, rawCost, templeCost);
        var middleCaseProfit = middleCaseValue is null
                                   ? Profit(ResultCase.Worst, rawCost, templeCost)
                                   : Profit((decimal)middleCaseValue, rawCost, templeCost);
        var bestCaseProfit = bestCaseValue is null
                                 ? Profit(ResultCase.Worst, rawCost, templeCost)
                                 : Profit((decimal)bestCaseValue, rawCost, templeCost);
        return (worstCaseProfit + 2 * middleCaseProfit + bestCaseProfit) / 4;
    }

    private decimal ResultValue(int level)
    {
        return Gems.Where(gem => gem.GemLevel == level && gem.Corrupted).MinBy(gem => gem.GemQuality)?.ChaosValue ?? 0m;
    }

    public bool IsExceptional()
    {
        return Name.Contains("Enhance") || Name.Contains("Empower") || Name.Contains("Enlighten");
    }

    public GemTradeData? ResultGem(ResultCase resultCase)
    {
        return Gems.Where(gem => gem.GemLevel == MaxLevel() + resultCase.LevelModifier()).MinBy(gem => gem.ChaosValue);
    }

    public GemTradeData? RawGem()
    {
        return Gems.Where(gem => gem.GemLevel == MaxLevel() && !gem.Corrupted).MinBy(gem => gem.ChaosValue);
    }
}