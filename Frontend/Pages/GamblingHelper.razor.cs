using Microsoft.AspNetCore.Components;
using PoEGamblingHelper3.Shared.Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper
{
    private IEnumerable<Gem> _gems = new[]
                                     {
                                         new Gem { Name = "Test Support" }, new Gem { Name = "Test Support 2" },
                                         new Gem { Name = "Test Support 3" }, new Gem { Name = "Test Support 4" }
                                     };

    [Inject] private IGemService GemService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _gems = await GemService.GetAllGems();
    }

    public async void LoadGamblingData() { Console.WriteLine("du dummer kek"); }
}