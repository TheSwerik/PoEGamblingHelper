using FluentAssertions;
using MockQueryable.Moq;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity;

namespace PoEGamblingHelper.Application.Test.Services;

public class LeagueServiceTest
{
    [Fact]
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
        var queryable = list.AsQueryable().BuildMockDbSet();
        var service = new LeagueService();

        service.GetCurrent(queryable.Object).Id.Should().Be(expectedId);
    }

    [Fact]
    public void GetCurrentLeagueEmptyTest()
    {
        var queryable = new List<League>().AsQueryable().BuildMockDbSet();
        var service = new LeagueService();

        Assert.Throws<NoLeagueDataException>(() => service.GetCurrent(queryable.Object));
    }
}