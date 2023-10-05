using Microsoft.AspNetCore.Components;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Shared.Layout.Components;

public partial class Footer : IAsyncDisposable
{
    [Inject] private IUpdateService UpdateService { get; set; } = null!;

    public async ValueTask DisposeAsync()
    {
        await UpdateService.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private string LastUpdateText()
    {
        return UpdateService.LastUpdate() == DateTime.MinValue
                   ? "Never"
                   : UpdateService.LastUpdate() < DateTime.Now.AddMinutes(-1)
                       ? $"{PassedMinutesSinceUpdate()} Minute{(PassedMinutesSinceUpdate() == 1 ? "" : "s")} ago"
                       : "Just now";
    }

    private int PassedMinutesSinceUpdate()
    {
        return (int)DateTime.Now.Subtract(UpdateService.LastUpdate()).TotalMinutes;
    }

    protected override void OnInitialized()
    {
        UpdateService.UpdateInterval = TimeSpan.FromMinutes(5);
        UpdateService.OnUiUpdate += async _ => await InvokeAsync(StateHasChanged);
        UpdateService.Init();
    }
}