﻿using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public partial class Filter : ComponentBase
{
    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public EventCallback<TempleCost> TempleCostChanged { get; set; }
    [Parameter] public FilterValues FilterValues { get; set; } = null!;
    [Parameter] public EventCallback<FilterValues> FilterValuesChanged { get; set; }
    [Parameter] public decimal ChaosPerDivine { get; set; }
    [Parameter] public EventCallback<decimal> ChaosPerDivineChanged { get; set; }

    private bool FiltersExpanded { get; set; } = false;

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        Console.WriteLine(args.Value);
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        TempleCost.ChaosValue = new[] { value };
        await TempleCostChanged.InvokeAsync(TempleCost);
    }

    private async Task UpdateChaosPerDivineChanged(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        ChaosPerDivine = value;
        await ChaosPerDivineChanged.InvokeAsync(ChaosPerDivine);
    }

    private async Task UpdateGemSearchText(ChangeEventArgs args)
    {
        if (args.Value?.ToString() is null) return;
        FilterValues.Gem = args.Value.ToString()!;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async Task UpdatePricePerTryFrom(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.PricePerTryFrom = value;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async Task UpdatePricePerTryTo(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.PricePerTryTo = value;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async Task UpdateGemType(ChangeEventArgs args)
    {
        if (args.Value is null || !Enum.TryParse<GemType>(args.Value.ToString(), out var value)) return;
        FilterValues.GemType = value;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async Task UpdateOnlyShowProfitable(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.OnlyShowProfitable = value;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async Task UpdateSort(ChangeEventArgs args)
    {
        if (args.Value is null || !Enum.TryParse<Sort>(args.Value.ToString(), out var value)) return;
        FilterValues.Sort = value;
        Console.WriteLine(FilterValues.Sort);
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async void ResetGemSearch()
    {
        FilterValues.Gem = string.Empty;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private async void ResetCostFilter()
    {
        FilterValues.PricePerTryFrom = decimal.MinValue;
        FilterValues.PricePerTryTo = decimal.MaxValue;
        await FilterValuesChanged.InvokeAsync(FilterValues);
    }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }

    private string ShowDecimal(decimal value)
    {
        return value is decimal.MinValue or decimal.MaxValue ? "" : value.Round(2);
    }

    private GemType[] GemTypes() { return Enum.GetValues<GemType>(); }
    private Sort[] Sorts() { return Enum.GetValues<Sort>(); }
}