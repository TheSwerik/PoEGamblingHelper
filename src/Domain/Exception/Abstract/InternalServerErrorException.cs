using Domain.Exception.Http;

namespace Domain.Exception.Abstract;

public abstract class InternalServerErrorException : HttpException
{
    public InternalServerErrorException(string message, PoeGamblingHelperExceptionBody body) : base(message, 500, body)
    {
    }
}