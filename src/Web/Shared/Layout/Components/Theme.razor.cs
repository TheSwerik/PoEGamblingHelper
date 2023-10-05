using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PoEGamblingHelper.Web.Shared.Layout.Components;

public partial class Theme
{
    private Themes _theme = Themes.Dark;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (Enum.TryParse(await JsRuntime.InvokeAsync<string>("getTheme"), true, out Themes theme)) _theme = theme;
    }

    private void FlipTheme()
    {
        var theme = _theme == Themes.Dark ? Themes.Light : Themes.Dark;
        if (_theme == theme) return;
        var js = (IJSInProcessRuntime)JsRuntime;
        js.InvokeVoid("setTheme", theme.ToString().ToLower());
        _theme = theme;
    }

    private enum Themes
    {
        Light,
        Dark
    }
}