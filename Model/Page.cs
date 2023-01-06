namespace Model;

public class Page
{
    private int? _pageSize;
    public int PageNumber { get; set; } = 0;

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value is null or <= 0 ? null : value;
    }

    public string ToQueryString() { return $"?pageNumber={PageNumber}&pageSize={_pageSize}"; }
}