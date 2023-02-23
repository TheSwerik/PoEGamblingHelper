using Domain.Entity;

namespace Application.Services;

public interface IDataFetchService
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData(League league);
    Task FetchTemplePriceData(League league);
    Task FetchGemPriceData(League league);
}