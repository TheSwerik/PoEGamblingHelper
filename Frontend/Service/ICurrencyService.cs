using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public interface ICurrencyService
{
    public Task<List<Currency>?> GetAll();
}