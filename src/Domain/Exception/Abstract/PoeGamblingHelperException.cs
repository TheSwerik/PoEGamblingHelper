namespace Domain.Exception.Abstract;

public abstract class PoeGamblingHelperException : System.Exception
{
    public PoeGamblingHelperException(string? message) : base(message) { }

    public PoeGamblingHelperException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }

    public override string ToString() { return $"{GetType()}: {Message}\n{StackTrace}"; }
}