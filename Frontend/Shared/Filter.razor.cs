using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Model;
using Model.QueryParameters;
using PoEGamblingHelper3.Shared.Model;

namespace PoEGamblingHelper3.Shared;

public partial class Filter : ComponentBase
{
    private readonly string[] _allowedFilterCurrencies =
        { "mirror-of-kalandra", "mirror-shard", "chaos-orb", "divine-orb" };

    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public FilterValues FilterValues { get; set; } = null!;
    [Parameter] public EventCallback<FilterValues> FilterValuesChanged { get; set; }
    [Parameter] public League CurrentLeague { get; set; } = null!;
    [Parameter] public List<Currency> Currency { get; set; } = new();
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private bool FiltersExpanded { get; set; } = false;

    private bool IsChaosSelected =>
        FilterValues.Currency is not null && FilterValues.Currency.Name.EqualsIgnoreCase("chaos orb");

    private async Task SaveFilterValues()
    {
        await FilterValuesChanged.InvokeAsync(FilterValues); //TODO create new backendQuery on change
        await LocalStorage.SetItemAsync("GemDataQuery", FilterValues);
    }

    private string TempleCostString()
    {
        var currencyChaosValue = FilterValues.CurrencyValue ?? FilterValues.Currency?.ChaosEquivalent ?? 1;
        var templeCost = FilterValues.TempleCost ?? TempleCost.AverageChaosValue();
        return ShowDecimal(templeCost / currencyChaosValue);
    }

    private string CurrencyValueString()
    {
        return FilterValues.CurrencyValue is null && FilterValues.Currency is not null
                   ? ShowDecimal(FilterValues.Currency.ChaosEquivalent)
                   : ShowDecimal(FilterValues.CurrencyValue);
    }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }

    private string ShowDecimal(decimal? value)
    {
        return value is null or decimal.MinValue or decimal.MaxValue ? "" : value.Round(2);
    }

    private GemType[] GemTypes() { return Enum.GetValues<GemType>(); }
    private Sort[] Sorts() { return Enum.GetValues<Sort>(); }

    private string TempleTradeUrl() { return TempleCost.TradeUrl(CurrentLeague); }

    private IEnumerable<Currency> GetAllowedFilterCurrencies()
    {
        return Currency.Where(currency => _allowedFilterCurrencies.Contains(currency.Id))
                       .OrderBy(currency => currency.ChaosEquivalent);
    }

    private string CurrencyName()
    {
        if (FilterValues.Currency is null || IsChaosSelected) return "";
        return $"Chaos per {FilterValues.Currency.Name}";
    }

    #region Update Callback

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        var currencyChaosValue = FilterValues.Currency?.ChaosEquivalent ?? 1;
        FilterValues.TempleCost = value * currencyChaosValue;
        await SaveFilterValues();
    }

    private async Task UpdateCurrencyValueChanged(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.CurrencyValue = value;
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

    private async Task UpdateGemType(GemType gemType)
    {
        FilterValues.GemType = gemType;
        await SaveFilterValues();
    }

    private async Task UpdateCurrency(string id)
    {
        var currency = Currency.FirstOrDefault(currency => currency.Id.Equals(id));
        if (currency is null) return;
        FilterValues.Currency = currency;
        FilterValues.CurrencyValue = null;
        await SaveFilterValues();
    }

    private async Task UpdateOnlyShowProfitable(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.OnlyShowProfitable = value;
        await SaveFilterValues();
    }

    private async Task UpdateSort(Sort sort)
    {
        FilterValues.Sort = sort;
        await SaveFilterValues();
    }

    #endregion

    #region Reset Inputs

    private async void ResetTempleCost()
    {
        FilterValues.TempleCost = null;
        await SaveFilterValues();
    }

    private async void ResetCurrencyValue()
    {
        FilterValues.CurrencyValue = null;
        await SaveFilterValues();
    }

    private async void ResetGemSearch()
    {
        FilterValues.Gem = string.Empty;
        await SaveFilterValues();
    }

    private async void ResetCostFilter()
    {
        FilterValues.PricePerTryFrom = null;
        FilterValues.PricePerTryTo = null;
        await SaveFilterValues();
    }

    #endregion
}