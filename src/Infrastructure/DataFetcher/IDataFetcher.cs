using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public interface IDataFetcher
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData(League league);
    Task FetchTemplePriceData(League league);
    Task FetchGemPriceData(League league);
}