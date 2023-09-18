using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Services;

public interface IDataFetchService //TODO rename
{
    Task FetchCurrentLeague();
    Task FetchCurrencyData(League league);
    Task FetchTemplePriceData(League league);
    Task FetchGemPriceData(League league);
}