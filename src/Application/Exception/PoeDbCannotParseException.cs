using PoEGamblingHelper.Application.Exception.Abstract;
using PoEGamblingHelper.Application.Exception.Body;

namespace PoEGamblingHelper.Application.Exception;

public class PoeDbCannotParseException(string message) : InternalServerErrorException(message,
                                                                                      new PoeGamblingHelperExceptionBody(
                                                                                          ExceptionType.InternalError,
                                                                                          ExceptionId.PoeDbCannotParse,
                                                                                          message
                                                                                      ));