namespace PoEGamblingHelper.Infrastructure.DataFetcher;

public interface IDataFetcher
{
    Task Fetch(string league);
}