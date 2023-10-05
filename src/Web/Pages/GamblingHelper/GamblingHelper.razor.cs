using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Web.Pages.GamblingHelper.Components.Filter;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Pages.GamblingHelper;

public partial class GamblingHelper : IDisposable
{
    private readonly List<GemData> _gems = new();
    private List<Currency> _currency = new();
    private League _currentLeague = new();
    private FilterModel _filterModel = new();
    private bool _firstLoad = true;
    private TempleCost _templeCost = new() { ChaosValue = new[] { 0m } };
    [Inject] private IGemService GemService { get; set; } = default!;
    [Inject] private ITempleCostService TempleCostService { get; set; } = default!;
    [Inject] private ICurrencyService CurrencyService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private ILeagueService LeagueService { get; set; } = default!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IUpdateService UpdateService { get; set; } = null!;

    public void Dispose()
    {
        UpdateService.OnUpdate -= async _ => await LoadGamblingData();
        UpdateService.OnUiUpdate -= async _ => await InvokeAsync(StateHasChanged);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var filterValues = await LocalStorage.GetItemAsync<FilterModel>("GemDataQuery");
        if (filterValues is not null) _filterModel = filterValues;

        UpdateService.OnUpdate += async _ => await LoadGamblingData();
        UpdateService.OnUiUpdate += async _ => await InvokeAsync(StateHasChanged);
        await UpdateService.Update();
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadGamblingData()
    {
        try
        {
            _firstLoad = false;
            await InvokeAsync(StateHasChanged);

            var currency = await CurrencyService.GetAll();
            if (currency is null || currency.Count == 0) return;
            _currency = currency;

            _filterModel.Currency = _filterModel.Currency is null
                                        ? _currency.First(c => c.Name.EqualsIgnoreCase("Divine Orb"))
                                        : _currency.FirstOrDefault(c => c.Id.Equals(_filterModel.Currency.Id));
            _filterModel.Currency ??= _currency.First(c => c.Name.EqualsIgnoreCase("Divine Orb"));


            var templeCost = await TempleCostService.Get();
            if (templeCost is not null) _templeCost = templeCost;

            var league = await LeagueService.GetCurrent();
            if (league is not null) _currentLeague = league;

            var gemPage = _currentGemPage;
            _isOnLastPage = false;
            _gems.Clear();
            for (var i = 0; i <= gemPage; i++)
            {
                _currentGemPage = i;
                await UpdateGems();
            }
        }
        catch (HttpRequestException)
        {
            _isOnLastPage = true;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
            await JsRuntime.InvokeVoidAsync("addTooltips");
        }
    }

    private async void OnFilterValuesChanged(FilterModel filterModel)
    {
        _filterModel = filterModel;
        _currentGemPage = 0;
        _isOnLastPage = false;
        await UpdateService.Update();
    }

    private async Task<bool> UpdateGems()
    {
        if (_isOnLastPage) return false;
        var gemPage = await GemService.GetAll(new PageRequest { PageSize = 20, PageNumber = _currentGemPage },
                                              _filterModel.ToQuery());
        if (gemPage is null) return false;

        _gems.AddRange(gemPage.Content);
        Console.WriteLine(_gems.Count);
        _currentGemPage = gemPage.CurrentPage;
        _isOnLastPage = gemPage.LastPage;
        return true;
    }

    #region OnScrollToBottom

    private readonly List<int> _positionsY = new();
    private int _currentGemPage;
    private bool _isOnLastPage;

    private async void OnScrollToBottom(object? sender, int positionY)
    {
        if (_positionsY.Contains(positionY)) return;
        _positionsY.Add(positionY);
        _currentGemPage++;
        if (!await UpdateGems())
        {
            _positionsY.Remove(positionY);
            _currentGemPage--;
        }

        await InvokeAsync(StateHasChanged);
        await JsRuntime.InvokeVoidAsync("addTooltips");
    }

    #endregion
}