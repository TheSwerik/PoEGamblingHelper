namespace PoEGamblingHelper.Application.QueryParameters;

public class PageRequest
{
    private readonly int _pageSize;
    public int PageNumber { get; init; }

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value <= 0 ? 0 : value;
    }

    public (int skipSize, int takeSize) ConvertToSizes() { return (PageSize * PageNumber, PageSize); }
}