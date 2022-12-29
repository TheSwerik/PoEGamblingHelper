using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public partial class Filter : ComponentBase
{
    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public decimal DivineValue { get; set; }
    [Parameter] public FilterValues FilterValues { get; set; } = null!;
    [Parameter] public EventCallback<FilterValues> FilterValuesChanged { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private bool FiltersExpanded { get; set; } = false;

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.TempleCost = value;
        await SaveFilterValues();
    }

    private async Task UpdateChaosPerDivineChanged(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.DivineValue = value;
        await SaveFilterValues();
    }

    private async Task UpdateGemSearchText(ChangeEventArgs args)
    {
        if (args.Value?.ToString() is null) return;
        FilterValues.Gem = args.Value.ToString()!;
        await SaveFilterValues();
    }

    private async Task UpdatePricePerTryFrom(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.PricePerTryFrom = value;
        await SaveFilterValues();
    }

    private async Task UpdatePricePerTryTo(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.PricePerTryTo = value;
        await SaveFilterValues();
    }

    private async Task UpdateGemType(ChangeEventArgs args)
    {
        if (args.Value is null || !Enum.TryParse<GemType>(args.Value.ToString(), out var value)) return;
        FilterValues.GemType = value;
        await SaveFilterValues();
    }

    private async Task UpdateOnlyShowProfitable(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.OnlyShowProfitable = value;
        await SaveFilterValues();
    }

    private async Task UpdateSort(ChangeEventArgs args)
    {
        if (args.Value is null || !Enum.TryParse<Sort>(args.Value.ToString(), out var value)) return;
        FilterValues.Sort = value;
        await SaveFilterValues();
    }

    private async void ResetTempleCost()
    {
        FilterValues.TempleCost = null;
        await SaveFilterValues();
    }

    private async void ResetDivineValue()
    {
        FilterValues.DivineValue = null;
        await SaveFilterValues();
    }

    private async void ResetGemSearch()
    {
        FilterValues.Gem = string.Empty;
        await SaveFilterValues();
    }

    private async void ResetCostFilter()
    {
        FilterValues.PricePerTryFrom = decimal.MinValue;
        FilterValues.PricePerTryTo = decimal.MaxValue;
        await SaveFilterValues();
    }

    private async Task SaveFilterValues()
    {
        await FilterValuesChanged.InvokeAsync(FilterValues);
        await LocalStorage.SetItemAsync("Filter", FilterValues);
    }

    private string TempleCostString() { return ShowDecimal(FilterValues.TempleCost ?? TempleCost.AverageChaosValue()); }

    private string DivineValueString() { return ShowDecimal(FilterValues.DivineValue ?? DivineValue); }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }

    private string ShowDecimal(decimal value)
    {
        return value is decimal.MinValue or decimal.MaxValue ? "" : value.Round(2);
    }

    private GemType[] GemTypes() { return Enum.GetValues<GemType>(); }
    private Sort[] Sorts() { return Enum.GetValues<Sort>(); }
}