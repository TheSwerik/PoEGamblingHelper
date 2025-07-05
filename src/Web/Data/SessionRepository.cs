using Microsoft.JSInterop;

namespace PoEGamblingHelper.Web.Data;

internal static class SessionRepository
{
    internal static async Task<Session> Create(IJSRuntime jsRuntime)
    {
        return await jsRuntime.InvokeAsync<Session>("createSession");
    }
}