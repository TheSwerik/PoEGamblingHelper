namespace PoEGamblingHelper2;

public record GamblingData
{
    public Item? Raw { get; set; }
    public Item? BestCase { get; set; }
    public Item? MediumCase { get; set; }
    public Item? WorstCase { get; set; }

    public string Name { get; set; }

    public decimal CostPerTry(decimal templeCost)
    {
        // if (Raw is null) throw new Exception("Raw Gem is NULL");
        if (Raw is null) return int.MinValue;
        return Raw.ExaltedValue + templeCost;
    }

    public decimal MinProfitPerTry(decimal templeCost)
    {
        // if (WorstCase is null) throw new Exception("WorstCase Gem is NULL");
        if (WorstCase is null) return int.MinValue;
        return WorstCase.ExaltedValue - CostPerTry(templeCost);
    }

    public decimal MaxProfitPerTry(decimal templeCost)
    {
        // if (BestCase is null) throw new Exception("BestCase Gem is NULL");
        if (BestCase is null) return int.MinValue;
        return BestCase.ExaltedValue - CostPerTry(templeCost);
    }

    public decimal ProbablyProfitPerTry(decimal templeCost)
    {
        // if (WorstCase is null || MediumCase is null || BestCase is null ) throw new Exception("MediumCase Gem is NULL");
        if (WorstCase is null || MediumCase is null || BestCase is null) return int.MinValue;
        return (WorstCase.ExaltedValue + MediumCase.ExaltedValue + BestCase.ExaltedValue) / 3 - CostPerTry(templeCost);
    }
}