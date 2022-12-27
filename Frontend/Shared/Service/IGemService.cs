using Model;

namespace PoEGamblingHelper3.Shared.Service;

public interface IGemService
{
    public Task<IEnumerable<GemData>> GetAllGems();
}