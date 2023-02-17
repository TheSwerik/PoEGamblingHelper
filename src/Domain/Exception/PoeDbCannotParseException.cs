using Domain.Exception.Abstract;
using Domain.Exception.Body;

namespace Domain.Exception;

public class PoeDbCannotParseException : InternalServerErrorException
{
    public PoeDbCannotParseException(string message) : base(message, new PoeGamblingHelperExceptionBody(
                                                                ExceptionType.InternalError,
                                                                ExceptionId.PoeDbCannotParse,
                                                                message
                                                            ))
    {
    }
}