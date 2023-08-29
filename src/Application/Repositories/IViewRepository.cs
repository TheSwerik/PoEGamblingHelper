using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Application.Repositories;

public interface IViewRepository
{
    Task AddAsync(View view);
    Task LogViewsAsync(DateOnly date);
}