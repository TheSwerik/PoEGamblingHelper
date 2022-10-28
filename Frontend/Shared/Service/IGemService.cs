using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared.Service;

public interface IGemService
{
    public Task<IEnumerable<Gem>> GetAllGems();
}