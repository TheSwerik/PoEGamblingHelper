namespace PoEGamblingHelper.Application.QueryParameters;

public class PageRequest
{
    private int _pageSize;
    public int PageNumber { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? 0 : value;
    }

    public (int skipSize, int takeSize) ConvertToSizes() { return (PageSize * PageNumber, PageSize); }
}