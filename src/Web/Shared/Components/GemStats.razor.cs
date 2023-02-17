using System.Diagnostics;
using Blazored.LocalStorage;
using Domain.Entity;
using Domain.Entity.Gem;
using Microsoft.AspNetCore.Components;
using Web.Shared.Model;
using Web.Util;

namespace Web.Shared.Components;

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

    private string TradeUrl(GemData gemData, ResultCase? resultCase = null)
    {
        if (resultCase is null) // raw gem
        {
            var rawUrlGem = new GemTradeData
                            {
                                Name = gemData.Name,
                                Corrupted = false,
                                GemLevel = gemData.MaxLevel(),
                                GemQuality = 0
                            };
            return rawUrlGem.TradeUrl(CurrentLeague);
        }

        var resultGem = GemData.ResultGem(resultCase.Value);
        if (resultGem is not null) return resultGem.TradeUrl(CurrentLeague);

        var levelModifier = resultCase switch
                            {
                                ResultCase.Worst => -1,
                                ResultCase.Middle => 0,
                                ResultCase.Best => 1,
                                _ => throw new UnreachableException()
                            };
        var urlGem = new GemTradeData
                     {
                         Name = gemData.Name,
                         Corrupted = true,
                         GemLevel = gemData.MaxLevel() + levelModifier,
                         GemQuality = 0
                     };
        return urlGem.TradeUrl(CurrentLeague);
    }

    private string CurrencyValue(decimal chaosValue) { return (chaosValue / CurrencyValue()).Round(2); }

    private decimal CurrencyValue()
    {
        return FilterValues.CurrencyValue
               ?? FilterValues.Currency?.ChaosEquivalent
               ?? 1;
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
        _values.RawValue = value * CurrencyValue();
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
        _values.WorstCaseValue = value * CurrencyValue();
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
        _values.MiddleCaseValue = value * CurrencyValue();
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
        _values.BestCaseValue = value * CurrencyValue();
        await SaveValues();
    }

    private async Task ResetValues()
    {
        _values = new Values();
        await LocalStorage.RemoveItemAsync(GemData.Id.ToString());
        _isEditing = false;
    }

    private async Task SaveValues() { await LocalStorage.SetItemAsync(GemData.Id.ToString(), _values); }

    protected override async Task OnInitializedAsync()
    {
        var values = await LocalStorage.GetItemAsync<Values>(GemData.Id.ToString());
        _values = values ?? new Values();
    }

    private string GetCurrencyString(decimal? value) { return value is null ? "" : CurrencyValue(value.Value); }

    private class Values
    {
        public decimal? RawValue { get; set; }
        public decimal? WorstCaseValue { get; set; }
        public decimal? MiddleCaseValue { get; set; }
        public decimal? BestCaseValue { get; set; }
    }
}