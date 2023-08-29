using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class HttpException : PoeGamblingHelperException
{
    protected HttpException(string message, int statusCode, PoeGamblingHelperExceptionBody? body) : base(message)
    {
        (StatusCode, Body) = (statusCode, body);
    }

    public int StatusCode { get; }
    public PoeGamblingHelperExceptionBody? Body { get; }
}