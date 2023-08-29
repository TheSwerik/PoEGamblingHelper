using PoEGamblingHelper.Domain.Entity;

namespace Web.Services.Interfaces;

public interface ITempleCostService
{
    public Task<TempleCost?> Get();
}