using Domain.QueryParameters;

namespace Domain.Test.QueryParameters;

public class PageRequestTest
{
    [Fact]
    public void SetPageSizeTest()
    {
        var pageRequest = new PageRequest
        {
            PageNumber = 0,
            PageSize = 0
        };

        pageRequest.PageSize = -1;
        pageRequest.PageSize.ShouldBe(0);

        pageRequest.PageSize = 0;
        pageRequest.PageSize.ShouldBe(0);

        pageRequest.PageSize = 1;
        pageRequest.PageSize.ShouldBe(1);
    }
}