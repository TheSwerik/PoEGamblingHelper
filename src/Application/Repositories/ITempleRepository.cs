using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Repositories;

public interface ITempleRepository
{
    TempleCost GetCurrent(string league);
}