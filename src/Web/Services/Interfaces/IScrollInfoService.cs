namespace PoEGamblingHelper.Web.Services.Interfaces;

public interface IScrollInfoService
{
    int ScrollY { get; }
    event Func<object?, int, Task> OnScrollToBottom;
}