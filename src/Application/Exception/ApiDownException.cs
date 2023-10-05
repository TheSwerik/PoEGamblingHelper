using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception;

public class ApiDownException : InternalServerErrorException
{
    public ApiDownException(string url, string message) : base($"{url} is down: {message}",
                                                               new PoeGamblingHelperExceptionBody(
                                                                   ExceptionType.InternalError,
                                                                   ExceptionId.PoeNinjaUnreachable
                                                               ))
    {
    }

    public ApiDownException(string url) : base($"{url} is down", new PoeGamblingHelperExceptionBody(
                                                   ExceptionType.InternalError,
                                                   ExceptionId.PoeNinjaUnreachable
                                               ))
    {
    }
}