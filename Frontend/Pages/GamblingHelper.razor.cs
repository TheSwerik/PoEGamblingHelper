using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper : IDisposable
{
    private IEnumerable<Currency> _currency = new List<Currency>();

    private IEnumerable<GemData> _gems = new[]
                                         {
                                             new GemData { Name = "Placeholder Support" },
                                             new GemData { Name = "Placeholder Support 2" },
                                             new GemData { Name = "Placeholder Support 3" },
                                             new GemData { Name = "Placeholder Support 4" }
                                         };

    private Task _getAllGems = null!;
    private bool _isUpdating;
    private DateTime _lastPoeNinjaUpdate = DateTime.MinValue;

    [Inject] private IGemService GemService { get; set; } = default!;

    public void Dispose() { _getAllGems.Dispose(); }
    private DateTime NextPoeNinjaUpdate() { return _lastPoeNinjaUpdate.AddMinutes(5); }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _getAllGems = Task.Run(async () =>
                               {
                                   while (true)
                                   {
                                       while (_isUpdating || NextPoeNinjaUpdate() > DateTime.Now)
                                           await Task.Delay(1000);
                                       await LoadGamblingData();
                                   }
                               });
    }

    public async Task LoadGamblingData()
    {
        _isUpdating = true;
        // _currency = await GemService.GetAllGems();
        _gems = await GemService.GetAllGems();
        _lastPoeNinjaUpdate = DateTime.Now;
        await InvokeAsync(StateHasChanged);
        _isUpdating = false;
        Console.WriteLine("Loaded new Data");
    }
}