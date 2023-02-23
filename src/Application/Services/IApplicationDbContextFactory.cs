namespace Application.Services;

public interface IApplicationDbContextFactory
{
    IApplicationDbContext CreateDbContext();
}