namespace Application.Services;

public interface IDataFetchService
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData();
    Task FetchTemplePriceData();
    Task FetchGemPriceData();
}