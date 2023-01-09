namespace Model;

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
    public decimal CostPerTry(decimal templeCost = 0) { return RawCost() + templeCost; }

    public decimal Profit(ResultCase resultCase, decimal templeCost = 0)
    {
        return Value(resultCase) - CostPerTry(templeCost);
    }

    public decimal AvgProfitPerTry(decimal templeCost = 0)
    {
        return (Profit(ResultCase.Worst, templeCost)
                + 2 * Profit(ResultCase.Middle, templeCost)
                + Profit(ResultCase.Best, templeCost))
               / 4;
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