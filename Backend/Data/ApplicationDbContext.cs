using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public virtual DbSet<Gem> Gems { get; init; } = null!;
    public virtual DbSet<League> Leagues { get; init; } = null!;
}