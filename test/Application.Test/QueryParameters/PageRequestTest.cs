using PoEGamblingHelper.Application.QueryParameters;

namespace PoEGamblingHelper.Application.Test.QueryParameters;

public class PageRequestTest
{
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void SetPageSizeTest(int size, int expected)
    {
        var pageRequest = new PageRequest { PageNumber = 0, PageSize = size };
        pageRequest.PageSize.ShouldBe(expected);
    }
}