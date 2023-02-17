namespace Web.Services.Interfaces;

public interface IScrollInfoService
{
    int ScrollY { get; }
    event EventHandler<int> OnScrollToBottom;
}