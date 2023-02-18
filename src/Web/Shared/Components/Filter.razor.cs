using System.Text.RegularExpressions;
using Blazored.LocalStorage;
using Domain.Entity;
using Domain.QueryParameters;
using Microsoft.AspNetCore.Components;
using Web.Shared.Model;
using Web.Util;

namespace Web.Shared.Components;

public partial class Filter : ComponentBase
{
    private readonly string[] _allowedFilterCurrencies =
        { "mirror-of-kalandra", "mirror-shard", "chaos-orb", "divine-orb" };

    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public FilterValues FilterValues { get; set; } = null!;
    [Parameter] public EventCallback<FilterValues> OnFilterValuesChanged { get; set; }
    [Parameter] public League CurrentLeague { get; set; } = null!;
    [Parameter] public List<Currency> Currency { get; set; } = new();
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private bool FiltersExpanded { get; set; }

    private bool IsChaosSelected =>
        FilterValues.Currency is not null &&
        FilterValues.Currency.Name.Equals("chaos orb", StringComparison.InvariantCultureIgnoreCase);

    private async Task SaveFilterValues()
    {
        await OnFilterValuesChanged.InvokeAsync(FilterValues);
        await LocalStorage.SetItemAsync("GemDataQuery", FilterValues);
    }

    private string TempleCostString()
    {
        var templeCost = FilterValues.TempleCost ?? TempleCost.AverageChaosValue();
        return CurrencyValue(templeCost);
    }

    private string CurrencyValueString()
    {
        return FilterValues.CurrencyValue is null && FilterValues.Currency is not null
                   ? ToStringOrBlank(FilterValues.Currency.ChaosEquivalent)
                   : ToStringOrBlank(FilterValues.CurrencyValue);
    }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }

    private string ToStringOrBlank(decimal? value)
    {
        return value is null or decimal.MinValue or decimal.MaxValue ? "" : value.Round(2)!;
    }

    private string CurrencyValue(decimal? value) { return ToStringOrBlank(value / ConversionRatio()); }

    private GemType[] GemTypes() { return Enum.GetValues<GemType>(); }
    private Sort[] Sorts() { return Enum.GetValues<Sort>(); }

    private string TempleTradeUrl()
    {
        const string poeTradeUrl = "https://www.pathofexile.com/trade/search";
        const string queryKey = "?q=";

        var query = JsonMinifyRegex().Replace(@"
        {
          ""query"":{
            ""stats"":[
              {
                ""type"":""and"",
                ""filters"":[
                  {
                    ""id"":""pseudo.pseudo_temple_gem_room_3"",
                    ""value"":{
                      ""option"":1
                    },
                    ""disabled"":false
                  }
                ]
              }
            ],
            ""type"": ""Chronicle of Atzoatl""
          }
        }
        ", "$1");
        return $"{poeTradeUrl}/{CurrentLeague.Name}{queryKey}{query}";
    }

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

    private decimal ConversionRatio()
    {
        return FilterValues.CurrencyValue ?? FilterValues.Currency?.ChaosEquivalent ?? 1;
    }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")] private static partial Regex JsonMinifyRegex();

    #region Update Callback

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.TempleCost = value * ConversionRatio();
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
        FilterValues.PricePerTryFrom = value * ConversionRatio();
        await SaveFilterValues();
    }

    private async Task UpdatePricePerTryTo(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.PricePerTryTo = value * ConversionRatio();
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

    private async Task UpdateShowAlternateQuality(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterValues.ShowAlternateQuality = value;
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