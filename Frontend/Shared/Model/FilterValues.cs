﻿using Model;

namespace PoEGamblingHelper3.Shared.Model;

public class FilterValues
{
    public string Gem { get; set; } = string.Empty;
    public Sort Sort { get; set; } = Sort.AverageProfitPerTryDesc;
    public GemType GemType { get; set; } = GemType.All;
    public decimal PricePerTryFrom { get; set; } = decimal.MinValue;
    public decimal PricePerTryTo { get; set; } = decimal.MaxValue;
    public bool OnlyShowProfitable { get; set; }
    public decimal CurrencyValue { get; set; } = 1;
    public decimal? TempleCost { get; set; } = null;
    public Currency? Currency { get; set; } = null;
}