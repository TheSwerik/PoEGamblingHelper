namespace Domain.QueryParameters;

public class PageRequest
{
    private int? _pageSize;
    public int PageNumber { get; set; }

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value is null or <= 0 ? null : value;
    }
}