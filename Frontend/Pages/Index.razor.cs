using Microsoft.AspNetCore.Components;
using PoEGamblingHelper2;

namespace PoEGamblingHelper3.Pages;

public partial class Index
{
    public List<GamblingData> GamblingData = new();
    [Inject] public GamblingCalculationService GamblingCalculationService { get; init; }

    public async void LoadGamblingData()
    {
        Console.WriteLine("du dummer kek");
        GamblingData = await GamblingCalculationService.GetCalculatedChances();
    }
}