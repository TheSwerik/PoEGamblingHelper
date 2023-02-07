namespace PoEGamblingHelper3.Service;

public interface IScrollInfoService
{
    int ScrollY { get; }
    event EventHandler<int> OnScrollToBottom;
}