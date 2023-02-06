using Shared.Exception;

namespace Backend.Exceptions;

public class PoeNinjaDownException : InternalServerErrorException
{
    public PoeNinjaDownException() : base(
        new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.PoeNinjaUnreachable))
    {
    }
}