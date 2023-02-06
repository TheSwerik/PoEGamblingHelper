using Shared.Exception;

namespace Backend.Exceptions;

public class NoLeagueDataException : NotFoundException
{
    public NoLeagueDataException() : base("No League Data exists",
                                          new PoeGamblingHelperExceptionBody(
                                              ExceptionType.NotFound, ExceptionId.NoLeagueData))
    {
    }
}