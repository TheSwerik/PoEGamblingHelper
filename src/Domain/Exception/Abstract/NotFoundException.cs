using Domain.Exception.Body;

namespace Domain.Exception.Abstract;

public abstract class NotFoundException : HttpException
{
    protected NotFoundException(string message, PoeGamblingHelperExceptionBody body) : base(message, 404, body) { }
}