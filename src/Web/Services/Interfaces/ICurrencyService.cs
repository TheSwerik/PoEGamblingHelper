using Domain.Entity;

namespace Web.Services.Interfaces;

public interface ICurrencyService
{
    public Task<List<Currency>?> GetAll();
}