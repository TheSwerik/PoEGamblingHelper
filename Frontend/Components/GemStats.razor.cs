using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PoEGamblingHelper3.Components.Model;
using Shared.Entity;

namespace PoEGamblingHelper3.Components;

public partial class GemStats
{
    private bool _isEditing = false;
    private Values _values = new();
    [Parameter] public GemData GemData { get; set; } = null!;

    [Parameter] public TempleCost TempleCost { get; set; } = null!;

    [Parameter] public FilterValues FilterValues { get; set; } = null!;

    [Parameter] public League CurrentLeague { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private decimal FilterTempleCost() { return FilterValues.TempleCost ?? TempleCost.AverageChaosValue(); }

    private string TradeUrl(GemTradeData? gemTradeData)
    {
        return gemTradeData is null ? "#" : gemTradeData.TradeUrl(CurrentLeague);
    }

    private string CurrencyValue(decimal chaosValue)
    {
        var currencyChaosValue = FilterValues.CurrencyValue ?? FilterValues.Currency?.ChaosEquivalent ?? 1;
        return (chaosValue / currencyChaosValue).Round(2);
    }

    private async Task UpdateRawValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _values.RawValue = null;
            await SaveValues();
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _values.RawValue = value * currencyChaosValue;
        await SaveValues();
    }

    private async Task UpdateWorstCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _values.WorstCaseValue = null;
            await SaveValues();
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _values.WorstCaseValue = value * currencyChaosValue;
        await SaveValues();
    }

    private async Task UpdateMiddleCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _values.MiddleCaseValue = null;
            await SaveValues();
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _values.MiddleCaseValue = value * currencyChaosValue;
        await SaveValues();
    }

    private async Task UpdateBestCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _values.BestCaseValue = null;
            await SaveValues();
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _values.BestCaseValue = value * currencyChaosValue;
        await SaveValues();
    }

    private async Task ResetValues()
    {
        _values = new Values();
        await LocalStorage.RemoveItemAsync(GemData.Id.ToString());
    }

    private async Task SaveValues() { await LocalStorage.SetItemAsync(GemData.Id.ToString(), _values); }

    protected override async Task OnInitializedAsync()
    {
        var values = await LocalStorage.GetItemAsync<Values>(GemData.Id.ToString());
        _values = values ?? new Values();
    }

    private string GetCurrencyString(decimal? value) { return value is null ? "" : CurrencyValue((decimal)value); }

    private class Values
    {
        public decimal? RawValue { get; set; }
        public decimal? WorstCaseValue { get; set; }
        public decimal? MiddleCaseValue { get; set; }
        public decimal? BestCaseValue { get; set; }
    }
}