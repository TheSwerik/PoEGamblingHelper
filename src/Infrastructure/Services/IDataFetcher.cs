using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.Services;

public interface IDataFetcher
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData(League league);
    Task FetchTemplePriceData(League league);
    Task FetchGemPriceData(League league);
}