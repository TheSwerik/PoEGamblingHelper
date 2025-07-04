using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Web.Extensions;
using PoEGamblingHelper.Web.Pages.GamblingHelper.Components.Filter;

namespace PoEGamblingHelper.Web.Pages.GamblingHelper.Components;

public partial class GemStats
{
    private bool _isEditing = false;
    private Values _values = new();
    [Parameter] public GemData GemData { get; set; } = null!;

    [Parameter] public TempleCost TempleCost { get; set; } = null!;

    [Parameter] public FilterModel FilterModel { get; set; } = null!;

    [Parameter] public League CurrentLeague { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private decimal FilterTempleCost()
    {
        return FilterModel.TempleCost ?? TempleCost.AverageChaosValue();
    }

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

        var resultGem = GemData.Gems
                               .Where(gem => gem.Corrupted &&
                                             gem.GemLevel == GemData.MaxLevel() + resultCase.Value.LevelModifier())
                               .MinBy(gem => gem.ChaosValue);
        if (resultGem is not null) return resultGem.TradeUrl(CurrentLeague);

        var urlGem = new GemTradeData
        {
            Name = gemData.Name,
            Corrupted = true,
            GemLevel = gemData.MaxLevel() + resultCase.Value.LevelModifier(),
            GemQuality = 0
        };
        return urlGem.TradeUrl(CurrentLeague);
    }

    private string CurrencyValue(decimal chaosValue)
    {
        return (chaosValue / CurrencyValue()).Round(2);
    }

    private decimal CurrencyValue()
    {
        return FilterModel.CurrencyValue ?? FilterModel.Currency?.ChaosEquivalent ?? 1;
    }

    private async Task UpdateRawValue(string? newValue)
    {
        Console.WriteLine(1);
        if (string.IsNullOrWhiteSpace(newValue))
        {
            _values.RawValue = null;
            await SaveValues();
            return;
        }

        Console.WriteLine(2);
        if (!decimal.TryParse(newValue, out var value)) return;
        Console.WriteLine(_values.RawValue);
        Console.WriteLine(3);
        _values.RawValue = (value * CurrencyValue()).ToString(CultureInfo.InvariantCulture);
        Console.WriteLine(4);
        Console.WriteLine(_values.RawValue);
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

    private async Task SaveValues()
    {
        await LocalStorage.SetItemAsync(GemData.Id.ToString(), _values);
    }

    protected override async Task OnInitializedAsync()
    {
        var values = await LocalStorage.GetItemAsync<Values>(GemData.Id.ToString());
        _values = values ?? new Values();
    }

    private string GetCurrencyString(decimal? value)
    {
        return value is null ? "" : CurrencyValue(value.Value);
    }

    private class Values
    {
        public string? RawValue { get; set; }
        public decimal? WorstCaseValue { get; set; }
        public decimal? MiddleCaseValue { get; set; }
        public decimal? BestCaseValue { get; set; }
    }
}