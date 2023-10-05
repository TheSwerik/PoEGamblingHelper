using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception;

public class NoLeagueDataException : NotFoundException
{
    public NoLeagueDataException() : base("No League Data exists", new PoeGamblingHelperExceptionBody(
                                              ExceptionType.NotFound,
                                              ExceptionId.NoLeagueData
                                          ))
    {
    }
}