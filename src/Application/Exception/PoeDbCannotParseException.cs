using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception;

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