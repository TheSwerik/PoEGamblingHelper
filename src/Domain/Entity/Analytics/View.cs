using Domain.Entity.Abstract;

namespace Domain.Entity.Analytics;

public class View : Entity<Guid>
{
    public byte[] IpHash { get; set; } = null!;
    public DateOnly TimeStamp { get; set; }
}