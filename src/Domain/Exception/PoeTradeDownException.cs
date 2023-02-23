using Domain.Exception.Abstract;
using Domain.Exception.Body;

namespace Domain.Exception;

public class PoeTradeDownException : InternalServerErrorException
{
    public PoeTradeDownException() : base("", new PoeGamblingHelperExceptionBody(
                                              ExceptionType.InternalError,
                                              ExceptionId.PoeTradeUnreachable
                                          ))
    {
    }
}