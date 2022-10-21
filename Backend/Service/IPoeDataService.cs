using Backend.Model;

namespace Backend.Service;

public interface IPoeDataService
{
    Task<League> GetCurrentLeague();
}