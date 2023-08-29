using System;
using PoEGamblingHelper.Domain.Entity.Abstract;

namespace PoEGamblingHelper.Domain.Entity.Analytics
{
    public class View : Entity<Guid>
    {
        public byte[] IpHash { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
    }
}