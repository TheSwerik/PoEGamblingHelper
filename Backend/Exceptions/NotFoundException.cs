using Shared.Exception;

namespace Backend.Exceptions;

public abstract class NotFoundException : HttpException
{
    public NotFoundException(string message, PoeGamblingHelperExceptionBody body) : base(message, 404, body) { }
}