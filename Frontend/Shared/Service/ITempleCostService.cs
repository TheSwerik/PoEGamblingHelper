using Model;

namespace PoEGamblingHelper3.Shared.Service;

public interface ITempleCostService
{
    public Task<TempleCost> Get();
}