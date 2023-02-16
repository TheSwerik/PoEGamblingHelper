using Application.Services;

namespace Infrastructure.Services;

public class DataFetchService : IDataFetchService
{
    public Task FetchCurrentLeague()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }

    public Task FetchCurrencyData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
        ;
    }

    public Task FetchTemplePriceData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }

    public Task FetchGemPriceData()
    {
        Console.WriteLine("NOT IMPLEMENTED");
        return Task.CompletedTask;
    }
}