using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class NotFoundException : HttpException
{
    protected NotFoundException(string message, PoeGamblingHelperExceptionBody body) : base(message, 404, body) { }
}