using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;
using PoEGamblingHelper.Infrastructure.Repositories;

namespace PoEGamblingHelper.Infrastructure.Test.Repositories;

public class LeagueRepositoryTest
{
    [Fact(Skip = "Cannot Mock ApplicationDbContext")] //TODO
    public void GetCurrentLeagueTest()
    {
        var expectedId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4);
        var today = DateTime.Today.ToUniversalTime();
        var list = new List<League>
        {
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1), StartDate = today.AddDays(-1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2), StartDate = today.AddMinutes(1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3), StartDate = today.AddDays(1) },
            new() { Id = expectedId, StartDate = today },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5), StartDate = today.AddSeconds(1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8), StartDate = today.AddMinutes(-1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6), StartDate = today.AddYears(1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7), StartDate = today.AddMonths(1) },
            new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9), StartDate = today }
        };

        #region Setup

        var appDbContextFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        var appDbContext = new Mock<ApplicationDbContext>();
        appDbContextFactory.Setup(f => f.CreateDbContext()).Returns(appDbContext.Object);
        var queryable = new List<League>().AsQueryable().BuildMockDbSet();
        appDbContext.Setup(a => a.League).Returns(queryable.Object);
        var dateTimeService = new Mock<IDateTimeService>();

        #endregion


        var service = new LeagueRepository(appDbContextFactory.Object, dateTimeService.Object);

        service.GetCurrent().Id.ShouldBe(expectedId);
    }

    [Fact(Skip = "Cannot Mock ApplicationDbContext")] //TODO
    public void GetCurrentLeagueEmptyTest()
    {
        #region Setup

        var appDbContextFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        var appDbContext =
            new Mock<ApplicationDbContext>(MockBehavior.Strict, new DbContextOptions<ApplicationDbContext>());
        appDbContextFactory.Setup(f => f.CreateDbContext()).Returns(appDbContext.Object);
        var queryable = new List<League>().AsQueryable().BuildMockDbSet();
        appDbContext.SetupGet(a => a.League).Returns(queryable.Object);
        var dateTimeService = new Mock<IDateTimeService>();

        #endregion

        var service = new LeagueRepository(appDbContextFactory.Object, dateTimeService.Object);

        Assert.Throws<NoLeagueDataException>(() => service.GetCurrent());
    }
}