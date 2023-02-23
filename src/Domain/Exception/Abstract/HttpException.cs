using Domain.Exception.Body;

namespace Domain.Exception.Abstract;

public abstract class HttpException : PoeGamblingHelperException
{
    protected HttpException(string message, int statusCode, PoeGamblingHelperExceptionBody? body) : base(message)
    {
        (StatusCode, Body) = (statusCode, body);
    }

    public int StatusCode { get; }
    public PoeGamblingHelperExceptionBody? Body { get; }
}