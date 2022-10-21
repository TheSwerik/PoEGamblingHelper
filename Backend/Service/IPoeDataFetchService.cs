namespace Backend.Service;

public interface IPoeDataFetchService : IDisposable
{
    Task GetCurrentLeague();
    Task GetPriceData();
}