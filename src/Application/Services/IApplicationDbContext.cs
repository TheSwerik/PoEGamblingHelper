using Domain.Entity;
using Domain.Entity.Gem;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface IApplicationDbContext : IDisposable
{
    DbSet<Currency> Currency { get; }
    DbSet<League> League { get; }
    DbSet<TempleCost> TempleCost { get; }
    DbSet<GemData> GemData { get; }
    DbSet<GemTradeData> GemTradeData { get; }
    Task<int> SaveChangesAsync();
    void ClearTrackedEntities();
}