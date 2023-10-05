using System.Collections.Immutable;

namespace PoEGamblingHelper.Application.QueryParameters;

public record Page<T>
{
    public IImmutableList<T> Content { get; init; } = null!;
    public int CurrentPage { get; init; }
    public bool LastPage { get; init; }
}