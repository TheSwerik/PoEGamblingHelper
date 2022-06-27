using Backend.Model;

namespace Backend.Service;

public abstract class Service
{
    protected readonly IRepository<Gem> _gemRepository;
    protected readonly ILogger<Service> _logger;

    protected Service(ILogger<Service> logger, IRepository<Gem> gemRepository)
    {
        _logger = logger;
        _gemRepository = gemRepository;
    }
}