using Application.Services;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.Test.Services;

public class GemServiceTest
{
    [Fact]
    public async Task GetAllNullQueryTest()
    {
        #region Setup

        var list = new List<GemData>
                   {
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7) },
                       new() { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9) }
                   };

        var queryable = list.AsQueryable().BuildMockDbSet();
        var appDbContextFactory = new Mock<IApplicationDbContextFactory>();
        var appDbContext = new Mock<IApplicationDbContext>();
        appDbContextFactory.Setup(f => f.CreateDbContext()).Returns(appDbContext.Object);
        appDbContext.Setup(a => a.GemData).Returns(queryable.Object);
        var service = new GemService(appDbContextFactory.Object);

        #endregion

        var pageRequest = new PageRequest { PageNumber = 0, PageSize = int.MaxValue };
        var result = await service.GetAll(null, pageRequest);
        result.Content.Count().Should().Be(list.Count);
        result.LastPage.Should().BeTrue();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 0, PageSize = 2 };
        result = await service.GetAll(null, pageRequest);
        result.Content.Count().Should().Be(pageRequest.PageSize);
        result.LastPage.Should().BeFalse();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 1, PageSize = 2 };
        result = await service.GetAll(null, pageRequest);
        result.Content.Count().Should().Be(pageRequest.PageSize);
        result.Content.Should().NotBeSameAs(list);
        result.LastPage.Should().BeFalse();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);

        pageRequest = new PageRequest { PageNumber = 1, PageSize = 5 };
        result = await service.GetAll(null, pageRequest);
        result.Content.Count().Should().Be(list.Count - pageRequest.PageSize);
        result.LastPage.Should().BeTrue();
        result.CurrentPage.Should().Be(pageRequest.PageNumber);
    }
}