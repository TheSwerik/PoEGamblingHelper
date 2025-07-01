using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class InternalServerErrorException(string message, PoeGamblingHelperExceptionBody body) : HttpException(message, 500, body);