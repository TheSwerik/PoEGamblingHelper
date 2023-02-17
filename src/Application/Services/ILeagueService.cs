using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public interface ILeagueService
{
    League GetCurrentLeague(DbSet<League> leagues);
}