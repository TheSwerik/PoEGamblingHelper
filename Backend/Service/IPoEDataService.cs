namespace Backend.Service;

public interface IPoEDataService : IDisposable
{
    Task GetCurrentLeague();
    Task GetPriceData();
}