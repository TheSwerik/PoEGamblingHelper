using Shared.Exception;

namespace Backend.Exceptions;

public class NoTempleDataException : NotFoundException
{
    public NoTempleDataException() : base("No Temple Data exists",
                                          new PoeGamblingHelperExceptionBody(
                                              ExceptionType.NotFound, ExceptionId.NoTempleData))
    {
    }
}