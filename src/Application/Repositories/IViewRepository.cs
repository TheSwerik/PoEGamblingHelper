using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Application.Repositories;

public interface IViewRepository
{
    Task AddAsync(View view);
    Task<int> CountViewsAsync(DateOnly date);
    Task RemoveAllAsync(DateOnly date);
}