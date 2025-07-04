using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface ITempleCostService
{
    public Task<TempleCost?> Get(string league);
}