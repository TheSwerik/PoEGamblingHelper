using Domain.Exception.Abstract;
using Domain.Exception.Http;

namespace Domain.Exception;

public class PoeNinjaDownException : InternalServerErrorException
{
    public PoeNinjaDownException() : base(
        "", new PoeGamblingHelperExceptionBody(ExceptionType.InternalError, ExceptionId.PoeNinjaUnreachable))
    {
    }
}