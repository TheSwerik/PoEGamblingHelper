using Domain.Exception.Abstract;
using Domain.Exception.Body;

namespace Domain.Exception;

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