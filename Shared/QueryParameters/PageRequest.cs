namespace Shared.QueryParameters;

public class PageRequest
{
    private int? _pageSize;
    public int PageNumber { get; set; }

    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value is null or <= 0 ? null : value;
    }

    public string ToQueryString()
    {
        return _pageSize is null ? $"?pageNumber={PageNumber}" : $"?pageNumber={PageNumber}&pageSize={_pageSize}";
    }

    public string ToQueryString(GemDataQuery? query)
    {
        return query is null ? ToQueryString() : query.ToQueryString(this);
    }
}