namespace Backend.Exceptions;

public abstract class PoeGamblingHelperException : Exception
{
    public PoeGamblingHelperException() { }

    public PoeGamblingHelperException(string? message) : base(message) { }

    public PoeGamblingHelperException(string? message, Exception? innerException) : base(message, innerException) { }
}