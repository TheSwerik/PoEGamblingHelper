namespace Model.QueryParameters;

public class Page<T>
{
    public IEnumerable<T> Content { get; init; }
    public int CurrentPage { get; init; }
    public bool LastPage { get; init; }
}