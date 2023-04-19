using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Analytics;

public class View
{
    [Key] public byte[] IpHash { get; set; }
    public DateOnly TimeStamp { get; set; }
}