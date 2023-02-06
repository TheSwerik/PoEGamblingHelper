namespace PoEGamblingHelper3.Shared.Service;

public interface IScrollInfoService
{
    int ScrollY { get; }
    event EventHandler<int> OnScrollToBottom;
}