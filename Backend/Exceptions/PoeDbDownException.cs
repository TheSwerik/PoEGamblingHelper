using Shared.Exception;

namespace Backend.Exceptions;

public class PoeDbDownException : InternalServerErrorException
{
    public PoeDbDownException() : base(
        "", new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.PoeDbUnreachable))
    {
    }
}