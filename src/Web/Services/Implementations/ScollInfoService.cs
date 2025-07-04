using Microsoft.JSInterop;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Services.Implementations;

public class ScrollInfoService : IScrollInfoService, IDisposable
{
    private readonly IJSRuntime _jsRuntime;

    public ScrollInfoService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _jsRuntime.InvokeVoidAsync("RegisterScrollInfoService", DotNetObjectReference.Create(this));
    }

    public async void Dispose()
    {
        await _jsRuntime.InvokeVoidAsync("UnRegisterScrollInfoService", DotNetObjectReference.Create(this));
    }

    public event Func<object?, int, Task>? OnScrollToBottom;

    public int ScrollY { get; private set; }

    [JSInvokable("OnScrollToBottom")]
    public void JsOnScrollToBottom(int scrollY)
    {
        ScrollY = scrollY;
        OnScrollToBottom?.Invoke(this, scrollY).RunSynchronously();
    }
}