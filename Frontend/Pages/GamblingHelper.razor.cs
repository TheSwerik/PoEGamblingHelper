using PoEGamblingHelper3.Shared.Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper
{
    private readonly GemService _gemService;
    private IEnumerable<Gem> _gems;

    public GamblingHelper(GemService gemService) { _gemService = gemService; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _gems = await _gemService.GetAllGems();
    }

    public async void LoadGamblingData() { Console.WriteLine("du dummer kek"); }
}