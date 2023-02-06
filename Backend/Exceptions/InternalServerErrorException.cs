using Shared.Exception;

namespace Backend.Exceptions;

public abstract class InternalServerErrorException : HttpException
{
    public InternalServerErrorException(PoeGamblingHelperExceptionBody body) : base(500, body) { }
}