namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class PoeGamblingHelperException(string? message) : System.Exception(message)
{
    public override string ToString()
    {
        return $"{GetType()}: {Message}\n{StackTrace}";
    }
}