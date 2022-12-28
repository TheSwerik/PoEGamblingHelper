using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public partial class Filter : ComponentBase
{
    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public EventCallback<TempleCost> TempleCostChanged { get; set; }
    [Parameter] public FilterValues FilterValues { get; set; } = null!;
    [Parameter] public EventCallback<FilterValues> FilterValuesChanged { get; set; }

    [Parameter] public List<GemData> Gems { get; set; } = null!;

    [Parameter] public decimal ChaosPerDivine { get; set; }

    private bool FiltersExpanded { get; set; } = false;

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        TempleCost.ChaosValue = new[] { value };
        await TempleCostChanged.InvokeAsync(TempleCost);
    }

    private async Task UpdateGemSearchText(ChangeEventArgs args)
    {
        if (args.Value?.ToString() is null) return;
        FilterValues.Gem = args.Value.ToString()!;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }
}