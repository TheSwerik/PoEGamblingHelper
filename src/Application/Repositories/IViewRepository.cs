namespace PoEGamblingHelper.Application.Repositories;

public interface IViewRepository
{
    Task AddAsync(string ipAddress);
    Task<int> CountViewsAsync(DateOnly date);
    Task RemoveAllAsync(DateOnly date);
}