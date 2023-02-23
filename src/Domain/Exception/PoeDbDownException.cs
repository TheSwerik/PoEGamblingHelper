using Domain.Exception.Abstract;
using Domain.Exception.Body;

namespace Domain.Exception;

public class PoeDbDownException : InternalServerErrorException
{
    public PoeDbDownException() : base("", new PoeGamblingHelperExceptionBody(
                                           ExceptionType.InternalError,
                                           ExceptionId.PoeDbUnreachable
                                       ))
    {
    }
}