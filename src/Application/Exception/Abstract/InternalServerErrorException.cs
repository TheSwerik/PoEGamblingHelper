using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class InternalServerErrorException : HttpException
{
    protected InternalServerErrorException(string message, PoeGamblingHelperExceptionBody body) : base(
        message, 500, body)
    {
    }
}