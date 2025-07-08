namespace PoEGamblingHelper.Application.Repositories;

public interface IViewRepository
{
    Task AddAsync(string ipAddress);
    Task<int> CountAsync(DateOnly date);
    Task RemoveAllAsync(DateOnly date);
}