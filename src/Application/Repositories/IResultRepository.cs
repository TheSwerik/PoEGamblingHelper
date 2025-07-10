using PoEGamblingHelper.Domain.Entity.Stats;

namespace PoEGamblingHelper.Application.Repositories;

public interface IResultRepository
{
    IAsyncEnumerable<Result> GetAll();
    Task<Result> SaveAsync(Result result);
}