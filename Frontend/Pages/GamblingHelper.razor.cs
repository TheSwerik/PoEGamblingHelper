using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper
{
    private IEnumerable<GemData> _gems = new[]
                                         {
                                             new GemData { Name = "Test Support" },
                                             new GemData { Name = "Test Support 2" },
                                             new GemData { Name = "Test Support 3" },
                                             new GemData { Name = "Test Support 4" }
                                         };

    [Inject] private IGemService GemService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _gems = await GemService.GetAllGems();
    }

    public async void LoadGamblingData() { Console.WriteLine("du dummer kek"); }
    public IEnumerable<IGrouping<string, GemData>> ConnectedGems() { return _gems.GroupBy(gem => gem.Name); }
}