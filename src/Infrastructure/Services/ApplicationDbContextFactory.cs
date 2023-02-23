using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ApplicationDbContextFactory : IApplicationDbContextFactory
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IApplicationDbContext CreateDbContext() { return _dbContextFactory.CreateDbContext(); }
}