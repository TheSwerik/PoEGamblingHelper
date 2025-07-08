using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface ICurrencyService
{
    public Task<List<Currency>?> GetAll(string league);
}