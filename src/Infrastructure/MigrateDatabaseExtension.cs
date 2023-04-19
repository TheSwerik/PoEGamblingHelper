using Application.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class MigrateDatabaseExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var contextFactory = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>();
        var context = (ApplicationDbContext)contextFactory.CreateDbContext();
        if (context.Database.IsNpgsql()) context.Database.Migrate();
    }
}