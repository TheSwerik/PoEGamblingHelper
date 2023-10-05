using Microsoft.AspNetCore.Components;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Pages;

public partial class Stats : IDisposable
{
    private readonly int[] _data = { Random.Shared.Next(10000), Random.Shared.Next(10000), Random.Shared.Next(10000) };
    private bool _isMyAccountSelected = true;
    [Inject] private IUpdateService UpdateService { get; set; } = null!;

    public void Dispose()
    {
        //UpdateService.OnUpdate -= async _ => await LoadGamblingData();
        UpdateService.OnUiUpdate -= async _ => await InvokeAsync(StateHasChanged);
        GC.SuppressFinalize(this);
    }

    private double LuckScore()
    {
        Console.WriteLine(_data[2]);
        return _data.Length > 0
                   ? _data[2] / (double)_data.Sum()
                   : 0;
    }

    protected override void OnInitialized()
    {
        //UpdateService.OnUpdate += async _ => await LoadGamblingData();
        UpdateService.OnUiUpdate += async _ => await InvokeAsync(StateHasChanged);
    }

    private string LuckAdjective()
    {
        return LuckScore() switch
               {
                   > 0.9 => "contacts at GGG",
                   > 0.7 => "a hacked client",
                   > 0.5 => "a streamer client",
                   > 0.4 => "amazing",
                   > 0.3 => "good",
                   > 0.26 => "decent",
                   > 0.24 => "average",
                   > 0.2 => "bad",
                   > 0.1 => "terrible",
                   > 0 => "abominable",
                   _ => "negative"
               };
    }
}