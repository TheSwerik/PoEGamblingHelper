using Domain.Entity;

namespace Application.Service;

public interface ICurrencyService
{
    IAsyncEnumerable<Currency> GetAllAsync();
}