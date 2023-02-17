using Domain.Exception.Body;

namespace Domain.Exception.Abstract;

public abstract class InternalServerErrorException : HttpException
{
    protected InternalServerErrorException(string message, PoeGamblingHelperExceptionBody body) : base(
        message, 500, body)
    {
    }
}