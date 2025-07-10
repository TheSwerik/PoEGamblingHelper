using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception.Abstract;

public abstract class NotFoundException(string message, PoeGamblingHelperExceptionBody body) : HttpException(message, 404, body);