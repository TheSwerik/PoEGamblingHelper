namespace PoEGamblingHelper.Application.Repositories;

public interface IAnalyticsDayRepository
{
    Task AddAsync(int viewCount, DateOnly date);
}