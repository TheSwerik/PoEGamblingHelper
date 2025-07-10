using PoEGamblingHelper.Application.QueryParameters;

namespace PoEGamblingHelper.Domain.Test.QueryParameters;

public class PageRequestTest
{
    [Fact]
    public void SetPageSizeTest()
    {
        var pageRequest = new PageRequest
        {
            PageNumber = 0,
            PageSize = -1
        };
        pageRequest.PageSize.ShouldBe(0);

        pageRequest = new PageRequest
        {
            PageNumber = 0,
            PageSize = 0
        };
        pageRequest.PageSize.ShouldBe(0);

        pageRequest = new PageRequest
        {
            PageNumber = 0,
            PageSize = 1
        };
        pageRequest.PageSize.ShouldBe(1);
    }
}