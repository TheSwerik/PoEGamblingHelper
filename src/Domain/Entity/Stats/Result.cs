using System;
using PoEGamblingHelper.Domain.Entity.Abstract;
using PoEGamblingHelper.Domain.Entity.Gem;

namespace PoEGamblingHelper.Domain.Entity.Stats
{
    public class Result : Entity<Guid>
    {
        public GemTradeData GemTradeData { get; set; }
        public decimal CurrencyValue { get; set; }
        public CurrencyResult CurrencyResult { get; set; }
    }
}