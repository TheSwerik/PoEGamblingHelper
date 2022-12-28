using Microsoft.AspNetCore.Components;
using Model;
using PoEGamblingHelper3.Shared.Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper : IDisposable
{
    private List<Currency> _currency = new();
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

    public void Dispose() { _loadGamblingDataTask.Dispose(); }
    private DateTime NextBackendUpdate() { return _lastBackendUpdate.AddMinutes(5); }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
        _gems = await GemService.GetAll();
        _lastBackendUpdate = DateTime.Now;

        await InvokeAsync(StateHasChanged);
        _isUpdating = false;
        Console.WriteLine("Loaded new Data");
    }

    public IEnumerable<GemData> FilteredGems()
    {
        var avgTempleCost = _templeCost.AverageChaosValue();
        return _gems
               .Where(gemData => gemData.Name.Contains(_filterValues.Gem, StringComparison.InvariantCultureIgnoreCase)
                                 && gemData.CostPerTry(avgTempleCost) >= _filterValues.PricePerTryFrom
                                 && gemData.CostPerTry(avgTempleCost) <= _filterValues.PricePerTryTo
                                 && ConformsToGemType(gemData, _filterValues.GemType))
               .OrderByDescending(gemData => gemData.AvgProfitPerTry(0))
               .Take(50);
    }

    private bool ConformsToGemType(GemData gemData, GemType gemType)
    {
        return gemType switch
               {
                   GemType.Awakened => gemData.Name.StartsWith("Awakened"),
                   GemType.Exceptional => gemData.IsExceptional(),
                   GemType.Skill => !gemData.Name.Contains("Support"),
                   GemType.RegularSupport => gemData.Name.Contains("Support"),
                   _ => true
               };
    }
}