using System;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity;

public class League : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string Version { get; set; } = null!;

    public override string ToString()
    {
        return $"[id={Id}, Version={Version}, Name={Name}, StartDate={StartDate.ToLongDateString()}]";
    }
}