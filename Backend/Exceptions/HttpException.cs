using Shared.Exception;

namespace Backend.Exceptions;

public abstract class HttpException : PoeGamblingHelperException
{
    protected HttpException(string message, int statusCode, PoeGamblingHelperExceptionBody? body) : base(message)
    {
        (StatusCode, Body) = (statusCode, body);
    }

    public int StatusCode { get; }
    public PoeGamblingHelperExceptionBody? Body { get; }
}