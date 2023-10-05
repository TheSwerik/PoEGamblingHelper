using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Web.Extensions;

namespace PoEGamblingHelper.Web.Pages.GamblingHelper.Components.Filter;

public partial class Filter : ComponentBase
{
    private readonly string[] _allowedFilterCurrencies =
        { "mirror-of-kalandra", "mirror-shard", "chaos-orb", "divine-orb" };

    [Parameter] public TempleCost TempleCost { get; set; } = null!;
    [Parameter] public FilterModel FilterModel { get; set; } = null!;
    [Parameter] public EventCallback<FilterModel> OnFilterValuesChanged { get; set; }
    [Parameter] public League CurrentLeague { get; set; } = null!;
    [Parameter] public List<Currency> Currency { get; set; } = new();
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

    private bool FiltersExpanded { get; set; }

    private bool IsChaosSelected =>
        FilterModel.Currency is not null &&
        FilterModel.Currency.Name.Equals("chaos orb", StringComparison.InvariantCultureIgnoreCase);

    private async Task SaveFilterValues()
    {
        await OnFilterValuesChanged.InvokeAsync(FilterModel);
        await LocalStorage.SetItemAsync("GemDataQuery", FilterModel);
    }

    private string TempleCostString()
    {
        var templeCost = FilterModel.TempleCost ?? TempleCost.AverageChaosValue();
        return CurrencyValue(templeCost);
    }

    private string CurrencyValueString()
    {
        return FilterModel.CurrencyValue is null && FilterModel.Currency is not null
                   ? ToStringOrBlank(FilterModel.Currency.ChaosEquivalent)
                   : ToStringOrBlank(FilterModel.CurrencyValue);
    }

    private void ToggleFilters() { FiltersExpanded = !FiltersExpanded; }

    private static string ToStringOrBlank(decimal? value)
    {
        return value is null or decimal.MinValue or decimal.MaxValue ? "" : value.Round(2)!;
    }

    private string CurrencyValue(decimal? value) { return ToStringOrBlank(value / ConversionRatio()); }

    private static IEnumerable<GemType> GemTypes() { return Enum.GetValues<GemType>(); }
    private static IEnumerable<Sort> Sorts() { return Enum.GetValues<Sort>(); }

    private string TempleTradeUrl()
    {
        const string poeTradeUrl = "https://www.pathofexile.com/trade/search";
        const string queryKey = "?q=";

        var query = """
                    {
                        "query":{
                            "stats":[
                                  {
                                    "type":"and",
                                    "filters":[
                                        {
                                            "id":"pseudo.pseudo_temple_gem_room_3",
                                            "value":{
                                              "option":1
                                            },
                                            "disabled":false
                                        }
                                      ]
                                  }
                            ],
                            "type": "Chronicle of Atzoatl"
                        }
                    }
                    """.ToQueryUrl();
        return $"{poeTradeUrl}/{CurrentLeague.Name}{queryKey}{query}";
    }

    private IEnumerable<Currency> GetAllowedFilterCurrencies()
    {
        return Currency.Where(currency => _allowedFilterCurrencies.Contains(currency.Id))
                       .OrderBy(currency => currency.ChaosEquivalent);
    }

    private string CurrencyName()
    {
        if (FilterModel.Currency is null || IsChaosSelected) return "";
        return $"Chaos per {FilterModel.Currency.Name}";
    }

    private decimal ConversionRatio()
    {
        return FilterModel.CurrencyValue ?? FilterModel.Currency?.ChaosEquivalent ?? 1;
    }

    #region Update Callback

    private async Task UpdateTempleCost(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.TempleCost = value * ConversionRatio();
        await SaveFilterValues();
    }

    private async Task UpdateCurrencyValueChanged(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.CurrencyValue = value;
        await SaveFilterValues();
    }

    private async Task UpdateGemSearchText(ChangeEventArgs args)
    {
        if (args.Value?.ToString() is null) return;
        FilterModel.Gem = args.Value.ToString()!;
        await SaveFilterValues();
    }

    private async Task UpdatePricePerTryFrom(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.PricePerTryFrom = value * ConversionRatio();
        await SaveFilterValues();
    }

    private async Task UpdatePricePerTryTo(ChangeEventArgs args)
    {
        if (args.Value is null || !decimal.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.PricePerTryTo = value * ConversionRatio();
        await SaveFilterValues();
    }

    private async Task UpdateGemType(GemType gemType)
    {
        FilterModel.GemType = gemType;
        await SaveFilterValues();
    }

    private async Task UpdateCurrency(string id)
    {
        var currency = Currency.FirstOrDefault(currency => currency.Id.Equals(id));
        if (currency is null) return;
        FilterModel.Currency = currency;
        FilterModel.CurrencyValue = null;
        await SaveFilterValues();
    }

    private async Task UpdateShowAlternateQuality(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.ShowAlternateQuality = value;
        await SaveFilterValues();
    }

    private async Task UpdateOnlyShowProfitable(ChangeEventArgs args)
    {
        if (args.Value is null || !bool.TryParse(args.Value.ToString(), out var value)) return;
        FilterModel.OnlyShowProfitable = value;
        await SaveFilterValues();
    }

    private async Task UpdateSort(Sort sort)
    {
        FilterModel.Sort = sort;
        await SaveFilterValues();
    }

    #endregion

    #region Reset Inputs

    private async void ResetTempleCost()
    {
        FilterModel.TempleCost = null;
        await SaveFilterValues();
    }

    private async void ResetCurrencyValue()
    {
        FilterModel.CurrencyValue = null;
        await SaveFilterValues();
    }

    private async void ResetGemSearch()
    {
        FilterModel.Gem = string.Empty;
        await SaveFilterValues();
    }

    private async void ResetCostFilter()
    {
        FilterModel.PricePerTryFrom = null;
        FilterModel.PricePerTryTo = null;
        await SaveFilterValues();
    }

    #endregion
}