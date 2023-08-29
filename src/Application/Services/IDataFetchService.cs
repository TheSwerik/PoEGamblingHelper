using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public interface IDataFetchService
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData(League league);
    Task FetchTemplePriceData(League league);
    Task FetchGemPriceData(League league);
}