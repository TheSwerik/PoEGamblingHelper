using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public interface IDataFetcher
{
    Task Fetch(League league);
}