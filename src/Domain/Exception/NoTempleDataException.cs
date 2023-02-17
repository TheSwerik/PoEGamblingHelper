using Domain.Exception.Abstract;
using Domain.Exception.Body;

namespace Domain.Exception;

public class NoTempleDataException : NotFoundException
{
    public NoTempleDataException() : base("No Temple Data exists", new PoeGamblingHelperExceptionBody(
                                              ExceptionType.NotFound,
                                              ExceptionId.NoTempleData
                                          ))
    {
    }
}