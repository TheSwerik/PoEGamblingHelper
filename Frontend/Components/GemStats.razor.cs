using Microsoft.AspNetCore.Components;
using PoEGamblingHelper3.Components.Model;
using Shared.Entity;

namespace PoEGamblingHelper3.Components;

public partial class GemStats
{
    private decimal? _bestCaseValue = null;

    private bool _isEditing = false;
    private decimal? _middleCaseValue = null;

    private decimal? _rawValue = null;
    private decimal? _worstCaseValue = null;
    [Parameter] public GemData GemData { get; set; } = null!;

    [Parameter] public TempleCost TempleCost { get; set; } = null!;

    [Parameter] public FilterValues FilterValues { get; set; } = null!;

    [Parameter] public League CurrentLeague { get; set; } = null!;

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

    private void UpdateRawValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _rawValue = null;
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _rawValue = value * currencyChaosValue;
    }

    private void UpdateWorstCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _worstCaseValue = null;
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _worstCaseValue = value * currencyChaosValue;
    }

    private void UpdateMiddleCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _middleCaseValue = null;
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _middleCaseValue = value * currencyChaosValue;
    }

    private void UpdateBestCaseValue(ChangeEventArgs args)
    {
        if (args.Value is null || string.IsNullOrWhiteSpace(args.Value.ToString()))
        {
            _bestCaseValue = null;
            return;
        }

        if (!decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        _bestCaseValue = value * currencyChaosValue;
    }

    private string GetCurrencyString(decimal? value) { return value is null ? "" : CurrencyValue((decimal)value); }
}