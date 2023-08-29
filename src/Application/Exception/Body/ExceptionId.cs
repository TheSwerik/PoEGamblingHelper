using System.Diagnostics;

namespace PoEGamblingHelper.Application.Exception.Body;

public static class ExceptionIdExtensions
{
    public static string ToIdString(this ExceptionId exceptionId)
    {
        return exceptionId switch
               { //wpV-gGA4PSk
                   ExceptionId.PoeTradeUnreachable => "YouShouldNeverSeeThis1",
                   ExceptionId.PoeDbUnreachable => "wpV-YouShouldNeverSeeThis2",
                   ExceptionId.PoeDbCannotParse => "YouShouldNeverSeeThis3",
                   ExceptionId.PoeNinjaUnreachable => "YouShouldNeverSeeThis4",
                   ExceptionId.CannotParseBackendException => "JT6kfgIkpjI",
                   ExceptionId.NoLeagueData => "o-YBDTqX_ZU",
                   ExceptionId.NoTempleData => "2spaHQDSaY8",
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
    CannotParseBackendException,
    NoLeagueData,
    NoTempleData
}