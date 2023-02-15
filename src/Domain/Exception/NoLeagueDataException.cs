using Domain.Exception.Abstract;
using Domain.Exception.Http;

namespace Domain.Exception;

public class NoLeagueDataException : NotFoundException
{
    public NoLeagueDataException() : base("No League Data exists",
                                          new PoeGamblingHelperExceptionBody(
                                              ExceptionType.NotFound, ExceptionId.NoLeagueData))
    {
    }
}