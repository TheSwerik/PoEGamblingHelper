namespace Model;

public class GemData : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public ICollection<GemTradeData> Gems { get; set; } = new List<GemTradeData>();

    public int MaxLevel()
    {
        var isAwakened = Name.Contains("Awakened");
        var isExceptional = Name.Contains("Enhance") || Name.Contains("Empower") || Name.Contains("Enlighten");
        return isAwakened && isExceptional ? 4 :
               isExceptional ? 3 :
               isAwakened ? 5 :
               20;
    }

    public override string ToString() { return $"[Id={Id}, Name={Name}, MaxLevel={MaxLevel()}]"; }

    public decimal RawCost()
    {
        return Gems.Where(gem => gem.GemLevel == MaxLevel() && !gem.Corrupted).MinBy(gem => gem.GemQuality)
                   ?.ChaosValue ?? 0m;
    }

    public decimal WorstCaseValue() { return ResultValue(MaxLevel() - 1); }
    public decimal MiddleCaseValue() { return ResultValue(MaxLevel()); }
    public decimal BestCaseValue() { return ResultValue(MaxLevel() + 1); }
    public decimal CostPerTry(decimal templeCost) { return RawCost() + templeCost; }
    public decimal WorstCaseProfit(decimal templeCost) { return WorstCaseValue() - CostPerTry(templeCost); }
    public decimal MiddleCaseProfit(decimal templeCost) { return MiddleCaseValue() - CostPerTry(templeCost); }
    public decimal BestCaseProfit(decimal templeCost) { return BestCaseValue() - CostPerTry(templeCost); }

    public decimal AvgProfitPerTry(decimal templeCost)
    {
        return (WorstCaseProfit(templeCost) + MiddleCaseProfit(templeCost) + BestCaseProfit(templeCost)) / 3;
    }

    private decimal ResultValue(int level)
    {
        return Gems.Where(gem => gem.GemLevel == level && gem.Corrupted).MinBy(gem => gem.GemQuality)?.ChaosValue ?? 0m;
    }
}