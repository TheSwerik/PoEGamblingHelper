using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception;

public class NoTempleDataException() : NotFoundException("No Temple Data exists",
                                                         new PoeGamblingHelperExceptionBody(
                                                             ExceptionType.NotFound,
                                                             ExceptionId.NoTempleData
                                                         ));