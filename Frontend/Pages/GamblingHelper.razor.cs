using System.Diagnostics;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared;
using PoEGamblingHelper3.Shared.Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper : IDisposable
{
    private List<Currency> _currency = new();
    private League _currentLeague = new();
    private decimal _divineValue;
    private FilterValues _filterValues = new();

    private List<GemData> _gems = new()
                                  {
                                      new GemData { Name = "Placeholder Support" },
                                      new GemData { Name = "Placeholder Support 2" },
                                      new GemData { Name = "Placeholder Support 3" },
                                      new GemData { Name = "Placeholder Support 4" }
                                  };

    private bool _isUpdating;
    private DateTime _lastBackendUpdate = DateTime.MinValue;
    private Task _loadGamblingDataTask = null!;
    private TempleCost _templeCost = new() { ChaosValue = new[] { 0m } };
    [Inject] private IGemService GemService { get; set; } = default!;
    [Inject] private ITempleCostService TempleCostService { get; set; } = default!;
    [Inject] private ICurrencyService CurrencyService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private ILeagueService LeagueService { get; set; } = default!;

    public void Dispose() { _loadGamblingDataTask.Dispose(); }
    private DateTime NextBackendUpdate() { return _lastBackendUpdate.AddMinutes(5); }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var filterValues = await LocalStorage.GetItemAsync<FilterValues>("Filter");
        if (filterValues is not null) _filterValues = filterValues;
        _loadGamblingDataTask = Task.Run(async () =>
                                         {
                                             while (true)
                                             {
                                                 while (_isUpdating || NextBackendUpdate() > DateTime.Now)
                                                     await Task.Delay(1000);
                                                 await LoadGamblingData();
                                             }
                                         });
    }

    public async Task LoadGamblingData()
    {
        _isUpdating = true;

        _currency = await CurrencyService.GetAll();
        _divineValue = _currency.Where(c => c.Name.Equals("Divine Orb")).Select(c => c.ChaosEquivalent).First();
        _templeCost = await TempleCostService.Get();
        _currentLeague = await LeagueService.GetCurrent();
        _gems = await GemService.GetAll();
        _lastBackendUpdate = DateTime.Now;

        await InvokeAsync(StateHasChanged);
        _isUpdating = false;
        Console.WriteLine("Loaded new Data");
    }

    public IEnumerable<GemData> FilteredGems()
    {
        var avgTempleCost = _filterValues.TempleCost ?? _templeCost.AverageChaosValue();
        return _gems
               .Where(gemData => gemData.Name.Contains(_filterValues.Gem, StringComparison.InvariantCultureIgnoreCase)
                                 && gemData.CostPerTry(avgTempleCost) >= _filterValues.PricePerTryFrom
                                 && gemData.CostPerTry(avgTempleCost) <= _filterValues.PricePerTryTo
                                 && gemData.ConformsToGemType(_filterValues.GemType)
                                 && (!_filterValues.OnlyShowProfitable || gemData.AvgProfitPerTry(avgTempleCost) > 0))
               .OrderBy(gemData => _filterValues.Sort switch
                                   {
                                       Sort.CostPerTryAsc => gemData.CostPerTry(avgTempleCost),
                                       Sort.CostPerTryDesc => -gemData.CostPerTry(avgTempleCost),
                                       Sort.AverageProfitPerTryAsc => gemData.AvgProfitPerTry(avgTempleCost),
                                       Sort.AverageProfitPerTryDesc => -gemData.AvgProfitPerTry(avgTempleCost),
                                       Sort.MaxProfitPerTryAsc => -gemData.Profit(avgTempleCost, ResultCase.Best),
                                       Sort.MaxProfitPerTryDesc => -gemData.Profit(avgTempleCost, ResultCase.Best),
                                       _ => throw new UnreachableException("Sort")
                                   })
               .Take(50);
    }
}