using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PoEGamblingHelper.Domain.Entity.Abstract;

[assembly: InternalsVisibleTo("Domain.Test")]

namespace PoEGamblingHelper.Domain.Entity.Gem;

public class GemData : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public IList<GemTradeData> Gems { get; set; } = new List<GemTradeData>();
}