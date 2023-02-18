using Domain.QueryParameters;
using FluentAssertions;

namespace Domain.Test;

public class TestPageRequest
{
    [Fact]
    public void TestSetPageSize()
    {
        var pageRequest = new PageRequest
                          {
                              PageNumber = 0,
                              PageSize = null
                          };

        pageRequest.PageSize = null;
        pageRequest.PageSize.Should().BeNull();

        pageRequest.PageSize = -1;
        pageRequest.PageSize.Should().BeNull();

        pageRequest.PageSize = 0;
        pageRequest.PageSize.Should().BeNull();

        pageRequest.PageSize = 1;
        pageRequest.PageSize.Should().Be(1);
    }
}