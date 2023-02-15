using Domain.Entity;

namespace Application.Service;

public interface ITempleService
{
    TempleCost? GetCurrent();
}