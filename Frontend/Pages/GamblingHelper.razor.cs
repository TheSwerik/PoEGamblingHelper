using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper
{
    private IEnumerable<GemData> _gems = new[]
                                         {
                                             new GemData { Name = "Placeholder Support" },
                                             new GemData { Name = "Placeholder Support 2" },
                                             new GemData { Name = "Placeholder Support 3" },
                                             new GemData { Name = "Placeholder Support 4" }
                                         };

    [Inject] private IGemService GemService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _gems = await GemService.GetAllGems();
    }

    public async void LoadGamblingData() { Console.WriteLine("du dummer kek"); }
}