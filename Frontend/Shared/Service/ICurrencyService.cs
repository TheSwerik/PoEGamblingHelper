using Model;

namespace PoEGamblingHelper3.Shared.Service;

public interface ICurrencyService
{
    public Task<List<Currency>> GetAll();
}