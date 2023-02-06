using System.Diagnostics;

namespace Shared.Exception;

public static class ExceptionIdExtensions
{
    public static string ToString(this ExceptionId exceptionId)
    {
        return exceptionId switch
               {
                   ExceptionId.PoeTradeUnreachable => "dQw4w9WgXcQ",
                   ExceptionId.PoeDbUnreachable => "wpV-gGA4PSk",
                   ExceptionId.PoeDbCannotParse => "2spaHQDSaY8",
                   ExceptionId.PoeNinjaUnreachable => "o-YBDTqX_ZU",
                   ExceptionId.CannotParseBackendException => "o-YBDTqX_ZU", //TODO
                   _ => throw new UnreachableException()
               };
    }
}

public enum ExceptionId
{
    PoeTradeUnreachable,
    PoeDbUnreachable,
    PoeDbCannotParse,
    PoeNinjaUnreachable,
    CannotParseBackendException
}