using Domain.Exception.Http;

namespace Domain.Exception.Abstract;

public abstract class NotFoundException : HttpException
{
    public NotFoundException(string message, PoeGamblingHelperExceptionBody body) : base(message, 404, body) { }
}