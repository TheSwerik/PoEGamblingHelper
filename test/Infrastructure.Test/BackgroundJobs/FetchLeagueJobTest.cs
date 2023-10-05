using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Infrastructure.BackgroundJobs;
using PoEGamblingHelper.Infrastructure.DataFetcher;

namespace PoEGamblingHelper.Infrastructure.Test.BackgroundJobs;

public class FetchLeagueJobTest
{
    [Fact]
    public async Task FetchCurrentLeagueTest()
    {
        var logger = new Mock<ILogger>();
        var dataFetchService = new Mock<ILeagueDataFetcher>();
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c.GetSection("BackgroundJobIntervals")).Returns(new MockConfigurationSection());
        var service = new FetchLeagueJob(logger.Object, dataFetchService.Object, configuration.Object);


        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));

        dataFetchService.Setup(s => s.Fetch()).Throws(() => new ApiDownException("poedb.tw"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.StartAsync(CancellationToken.None)));

        dataFetchService.Setup(s => s.Fetch()).Throws<UnreachableException>();
        await Assert.ThrowsAsync<UnreachableException>(async () => await service.StartAsync(CancellationToken.None));
    }

    private class MockConfigurationSection : IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren() { throw new NotImplementedException(); }

        public IChangeToken GetReloadToken() { throw new NotImplementedException(); }

        public IConfigurationSection GetSection(string key)
        {
            if (key.Equals(nameof(FetchLeagueJob))) return new MockConfigurationSection { Value = "06:00:00" };
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