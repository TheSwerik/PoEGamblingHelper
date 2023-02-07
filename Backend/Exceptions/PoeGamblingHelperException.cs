namespace Backend.Exceptions;

public abstract class PoeGamblingHelperException : Exception
{
    public PoeGamblingHelperException(string? message) : base(message) { }

    public PoeGamblingHelperException(string? message, Exception? innerException) : base(message, innerException) { }

    public override string ToString() { return $"{GetType()}: {Message}\n{StackTrace}"; }
}