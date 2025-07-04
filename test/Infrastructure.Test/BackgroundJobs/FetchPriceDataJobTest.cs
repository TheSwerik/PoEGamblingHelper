using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.BackgroundJobs;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.Test.BackgroundJobs;

public class FetchPriceDataJobTest
{
    [Fact]
    public async Task FetchPriceDataTest()
    {
        #region Creation

        var league = new League { Name = "Standard" };
        var logger = new Mock<ILogger<FetchPriceDataJob>>();
        var cache = new Mock<IOutputCacheStore>();
        var leagueRepository = new Mock<ILeagueRepository>();
        leagueRepository.Setup(s => s.GetCurrent()).Returns(league);
        var currencyDataFetcher = new Mock<IDataFetcher>();
        var templeDataFetcher = new Mock<IDataFetcher>();
        var gemDataFetcher = new Mock<IDataFetcher>();
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("BackgroundJobIntervals")).Returns(new MockConfigurationSection());

        var service = new FetchPriceDataJob(logger.Object,
                                            cache.Object,
                                            leagueRepository.Object,
                                            configuration.Object,
                                            currencyDataFetcher.Object,
                                            templeDataFetcher.Object,
                                            gemDataFetcher.Object);

        #endregion

        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));

        leagueRepository.Verify();
        currencyDataFetcher.Verify();
        templeDataFetcher.Verify();
        gemDataFetcher.Verify();
        cache.Verify();


        leagueRepository.Setup(s => s.GetCurrent()).Throws<NoLeagueDataException>();
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));

        leagueRepository.Invocations.Clear();
        leagueRepository.Setup(s => s.GetCurrent()).Returns(league);

        currencyDataFetcher.Setup(s => s.Fetch(It.IsAny<string>()))
                           .Throws(() => new ApiDownException("poedb.tw"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));
        currencyDataFetcher.Setup(s => s.Fetch(It.IsAny<string>())).Throws(new PoeDbCannotParseException(""));
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));

        templeDataFetcher.Setup(s => s.Fetch(It.IsAny<string>()))
                         .Throws(() => new ApiDownException("pathofexile.com/trade"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));
        templeDataFetcher.Invocations.Clear();

        gemDataFetcher.Setup(s => s.Fetch(It.IsAny<string>()))
                      .Throws(() => new ApiDownException("poe.ninja"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));
    }

    private class MockConfigurationSection : IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            if (key.Equals(nameof(FetchPriceDataJob))) return new MockConfigurationSection { Value = "06:00:00" };
            throw new NotImplementedException();
        }

        public string? this[string key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string Key { get; }
        public string Path { get; }
        public string? Value { get; set; }
    }
}