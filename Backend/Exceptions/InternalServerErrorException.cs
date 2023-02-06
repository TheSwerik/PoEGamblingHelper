using Shared.Exception;

namespace Backend.Exceptions;

public abstract class InternalServerErrorException : HttpException
{
    public InternalServerErrorException(string message, PoeGamblingHelperExceptionBody body) : base(message, 500, body)
    {
    }
}