﻿using Microsoft.JSInterop;

namespace PoEGamblingHelper3.Service;

public class ScrollInfoService : IScrollInfoService, IDisposable
{
    private readonly IJSRuntime _jsRuntime;

    public ScrollInfoService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _jsRuntime.InvokeVoidAsync("RegisterScrollInfoService", DotNetObjectReference.Create(this));
    }

    public void Dispose()
    {
        _jsRuntime.InvokeVoidAsync("UnRegisterScrollInfoService", DotNetObjectReference.Create(this));
    }

    public event EventHandler<int>? OnScrollToBottom;

    public int ScrollY { get; private set; }

    [JSInvokable("OnScrollToBottom")]
    public void JsOnScrollToBottom(int scrollY)
    {
        ScrollY = scrollY;
        OnScrollToBottom?.Invoke(this, scrollY);
    }
}