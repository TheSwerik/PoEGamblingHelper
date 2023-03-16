using System.Diagnostics;
using Application.Services;
using Domain.Entity;
using Domain.Exception;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Moq;

#pragma warning disable CS8625

namespace Application.Test.Services;

public class InitServiceTest
{
    [Fact]
    public async Task FetchCurrentLeagueTest()
    {
        var logger = new Mock<ILogger<InitService>>();
        var dataFetchService = new Mock<IDataFetchService>();
        var service = new InitService(logger.Object, dataFetchService.Object, null, TimeSpan.Zero, null, null, null);

        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchCurrentLeague()));

        dataFetchService.Setup(s => s.FetchCurrentLeague()).Throws(() => new ApiDownException("poedb.tw"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchCurrentLeague()));

        dataFetchService.Setup(s => s.FetchCurrentLeague()).Throws<UnreachableException>();
        await Assert.ThrowsAsync<UnreachableException>(async () => await service.FetchCurrentLeague());
    }

    [Fact]
    public async Task FetchPriceDataTest()
    {
        #region Creation

        var logger = new Mock<ILogger<InitService>>();
        var dataFetchService = new Mock<IDataFetchService>();
        var cache = new Mock<IOutputCacheStore>();
        var appDbContextFactory = new Mock<IApplicationDbContextFactory>();
        var appDbContext = new Mock<IApplicationDbContext>();
        appDbContextFactory.Setup(f => f.CreateDbContext()).Returns(appDbContext.Object);
        var leagueService = new Mock<ILeagueService>();

        leagueService.Setup(s => s.GetCurrentLeague(appDbContext.Object.League)).Verifiable();
        dataFetchService.Setup(s => s.FetchCurrencyData(null)).Verifiable();
        dataFetchService.Setup(s => s.FetchTemplePriceData(null)).Verifiable();
        dataFetchService.Setup(s => s.FetchGemPriceData(null)).Verifiable();
        cache.Setup(c => c.EvictByTagAsync(null, new CancellationToken())).Verifiable();

        var service = new InitService(logger.Object, dataFetchService.Object, cache.Object, TimeSpan.Zero, null,
                                      appDbContextFactory.Object, leagueService.Object);

        #endregion

        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));

        leagueService.Verify();
        dataFetchService.Verify();
        cache.Verify();


        leagueService.Setup(s => s.GetCurrentLeague(appDbContext.Object.League)).Throws<NoLeagueDataException>();
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));

        leagueService.Invocations.Clear();
        leagueService.Setup(s => s.GetCurrentLeague(appDbContext.Object.League)).Returns(new League());

        dataFetchService.Setup(s => s.FetchCurrencyData(It.IsAny<League>()))
                        .Throws(() => new ApiDownException("poedb.tw"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));
        dataFetchService.Setup(s => s.FetchCurrencyData(It.IsAny<League>())).Throws(new PoeDbCannotParseException(""));
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));

        dataFetchService.Setup(s => s.FetchTemplePriceData(It.IsAny<League>()))
                        .Throws(() => new ApiDownException("pathofexile.com/trade"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));
        dataFetchService.Invocations.Clear();

        dataFetchService.Setup(s => s.FetchGemPriceData(It.IsAny<League>()))
                        .Throws(() => new ApiDownException("poe.ninja"));
        Assert.Null(await Record.ExceptionAsync(async () => await service.FetchPriceData()));
    }
}