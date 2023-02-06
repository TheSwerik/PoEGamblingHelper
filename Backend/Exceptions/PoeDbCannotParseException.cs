using Shared.Exception;

namespace Backend.Exceptions;

public class PoeDbCannotParseException : InternalServerErrorException
{
    public PoeDbCannotParseException(string message) : base(message,
                                                            new PoeGamblingHelperExceptionBody(
                                                                ExceptionType.InternalError,
                                                                ExceptionId.PoeDbCannotParse, message))
    {
    }
}