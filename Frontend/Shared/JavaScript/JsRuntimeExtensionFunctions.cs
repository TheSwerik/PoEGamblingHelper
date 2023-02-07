using Microsoft.JSInterop;

namespace PoEGamblingHelper3.Shared.JavaScript;

public static class JsRuntimeExtensionFunctions
{
    // await JsRuntime.InvokeVoidAsync("useTheme",Theme.Light);
    public static async ValueTask InvokeVoidAsync(this IJSRuntime jsRuntime, string identifier, Enum parameter)
    {
        Console.WriteLine(parameter);
        Console.WriteLine(parameter.ToString());
        Console.WriteLine(parameter.ToString().ToLowerInvariant());
        await jsRuntime.InvokeVoidAsync(identifier, parameter.ToString().ToLowerInvariant());
    }
}