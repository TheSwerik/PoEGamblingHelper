// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace PoEGamblingHelper.Web.Pages.Statistics;

internal record Session(int Id, DateTime TimeStamp, ResultEntry[] Results);

internal record ResultEntry(
    int Id,
    int SessionId,
    DateTime Timestamp,
    int GemId,
    Result Result,
    decimal GemCost,
    decimal TempleCost,
    decimal ResultPrice
);

internal enum Result
{
    RemoveLevel = -1,
    KeepLevel = 0,
    AddLevel = 1
}