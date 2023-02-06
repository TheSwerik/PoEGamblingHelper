namespace Shared.QueryParameters;

public class Page<T>
{
    public IEnumerable<T> Content { get; init; } = null!;
    public int CurrentPage { get; init; }
    public bool LastPage { get; init; }
}