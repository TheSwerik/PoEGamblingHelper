using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class HttpException(string message, int statusCode, PoeGamblingHelperExceptionBody? body) : PoeGamblingHelperException(message)
{
    public int StatusCode { get; } = statusCode;
    public PoeGamblingHelperExceptionBody? Body { get; } = body;
}