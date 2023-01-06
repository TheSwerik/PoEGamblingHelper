using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Model;
using Model.QueryParameters;
using PoEGamblingHelper3.Shared.Model;
using PoEGamblingHelper3.Shared.Service;

namespace PoEGamblingHelper3.Pages;

public partial class GamblingHelper : IDisposable
{
    private List<Currency> _currency = new();
    private League _currentLeague = new();
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
        var filterValues = await LocalStorage.GetItemAsync<FilterValues>("GemDataQuery");
        if (filterValues is not null) _filterValues = filterValues;
        _loadGamblingDataTask = Task.Run(async () =>
                                         {
                                             while (true)
                                             {
                                                 while (_isUpdating || NextBackendUpdate() > DateTime.Now)
                                                 {
                                                     await InvokeAsync(StateHasChanged);
                                                     await Task.Delay(1000);
                                                 }

                                                 await LoadGamblingData();
                                                 await InvokeAsync(StateHasChanged);
                                             }
                                         });
    }

    public async Task LoadGamblingData()
    {
        _isUpdating = true;

        _currency = await CurrencyService.GetAll();
        _filterValues.Currency = _filterValues.Currency is null
                                     ? _currency.First(c => c.Name.Equals("Divine Orb"))
                                     : _currency.FirstOrDefault(c => c.Id.Equals(_filterValues.Currency.Id));
        _filterValues.Currency ??= _currency.First(c => c.Name.Equals("Divine Orb"));
        _templeCost = await TempleCostService.Get();
        _currentLeague = await LeagueService.GetCurrent();
        _gems = await GemService.GetAll(new Page { PageSize = 20 }, _filterValues.ToQuery());
        _lastBackendUpdate = DateTime.Now;

        await InvokeAsync(StateHasChanged);
        _isUpdating = false;
        Console.WriteLine("Loaded new Data");
    }
}