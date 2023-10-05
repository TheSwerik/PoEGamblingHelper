using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Repositories;

public interface ICurrencyRepository
{
    IAsyncEnumerable<Currency> GetAll();
}