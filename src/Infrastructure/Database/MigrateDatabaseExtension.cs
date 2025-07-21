using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PoEGamblingHelper.Infrastructure.Database;

public static class MigrateDatabaseExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using var context = contextFactory.CreateDbContext();
        if (context.Database.IsNpgsql()) context.Database.Migrate();
    }
}