using Shared.Entity;

namespace PoEGamblingHelper3.Service;

public interface ITempleCostService
{
    public Task<TempleCost?> Get();
}