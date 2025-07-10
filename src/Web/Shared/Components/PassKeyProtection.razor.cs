using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Shared.Components;

public partial class PassKeyProtection
{
    private string _apiKey = "";
    private bool _isLoggedIn;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Inject] private IAnalyticsService AnalyticsService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _isLoggedIn = await AnalyticsService.Check();
    }

    private async Task Login()
    {
        await AnalyticsService.Login(_apiKey);
        _isLoggedIn = await AnalyticsService.Check();
    }

    private async Task Submit(KeyboardEventArgs arg)
    {
        if (arg.Key == "Enter") await Login();
    }
}