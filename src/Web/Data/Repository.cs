using Microsoft.JSInterop;

namespace PoEGamblingHelper.Web.Data;

internal static class Repository
{
    internal static async Task<Session> CreateSession(IJSRuntime jsRuntime)
    {
        return await jsRuntime.InvokeAsync<Session>("createSession");
    }
}