using Shared.Exception;

namespace Backend.Exceptions;

public class PoeTradeDownException : InternalServerErrorException
{
    public PoeTradeDownException() : base(
        new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.PoeTradeUnreachable))
    {
    }
}