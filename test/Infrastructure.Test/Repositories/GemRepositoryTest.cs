using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure.Database;
using PoEGamblingHelper.Infrastructure.Repositories;

namespace PoEGamblingHelper.Infrastructure.Test.Repositories;

public class GemRepositoryTest
{
    [Fact(Skip = "Cannot Mock ApplicationDbContext")] //TODO
    public void GetAllNullQueryTest()
    {
        #region Setup

        var list = new List<GemData> { new(), new(), new(), new(), new(), new(), new(), new(), new() };
        var queryable = list.AsQueryable().BuildMockDbSet();

        var appDbContext = new Mock<ApplicationDbContext>();
        appDbContext.SetupGet(c => c.GemData).Returns(queryable.Object);
        var appDbContextFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        appDbContextFactory.Setup(f => f.CreateDbContext()).Returns(appDbContext.Object);
        var templeRepository = new Mock<ITempleRepository>();
        var service = new GemRepository(appDbContextFactory.Object, templeRepository.Object);

        #endregion

        var pageRequest = new PageRequest { PageNumber = 0, PageSize = int.MaxValue };
        var result = service.Search(null, pageRequest);
        result.Content.Count.Should().Be(list.Count);
        result.LastPage.Should().BeTrue();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 0, PageSize = 2 };
        result = service.Search(null, pageRequest);
        result.Content.Count.Should().Be(pageRequest.PageSize);
        result.LastPage.Should().BeFalse();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 1, PageSize = 2 };
        result = service.Search(null, pageRequest);
        result.Content.Count.Should().Be(pageRequest.PageSize);
        result.Content.Should().NotBeSameAs(list);
        result.LastPage.Should().BeFalse();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 1, PageSize = 5 };
        result = service.Search(null, pageRequest);
        result.Content.Count.Should().Be(list.Count - pageRequest.PageSize);
        result.LastPage.Should().BeTrue();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);
    }
}