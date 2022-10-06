namespace Backend.Service;

public abstract class Service : IDisposable
{
    protected readonly ILogger<Service> Logger;
    protected readonly IServiceScope Scope;

    protected Service(ILogger<Service> logger, IServiceScopeFactory serviceScopeFactory)
    {
        Logger = logger;
        Scope = serviceScopeFactory.CreateScope();
    }

    public void Dispose() { Scope.Dispose(); }
}