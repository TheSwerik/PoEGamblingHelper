namespace Domain.Exception.Abstract;

public abstract class PoeGamblingHelperException : System.Exception
{
    protected PoeGamblingHelperException(string? message) : base(message) { }
    public override string ToString() { return $"{GetType()}: {Message}\n{StackTrace}"; }
}