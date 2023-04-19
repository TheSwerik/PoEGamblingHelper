using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Analytics;

public class View
{
    [Key] public byte[] IpHash { get; set; } = null!;
    public DateOnly TimeStamp { get; set; }
}