using Blazored.LocalStorage;
using Domain.Entity;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Web.Services.Interfaces;
using Web.Shared.Model;

namespace Web.Pages;

public partial class GamblingHelper : IAsyncDisposable
{
    private readonly List<GemData> _gems = new();
    private List<Currency> _currency = new();
    private League _currentLeague = new();
    private FilterValues _filterValues = new();
    private bool _firstLoad = true;
    private bool _isUpdating;
    private DateTime _lastBackendUpdate = DateTime.MinValue;
    private Task _loadGamblingDataTask = null!;
    private TempleCost _templeCost = new() { ChaosValue = new[] { 0m } };
    [Inject] private IGemService GemService { get; set; } = default!;
    [Inject] private ITempleCostService TempleCostService { get; set; } = default!;
    [Inject] private ICurrencyService CurrencyService { get; set; } = default!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] private ILeagueService LeagueService { get; set; } = default!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

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
                                                     _tokenSource.Token.ThrowIfCancellationRequested();
                                                     await InvokeAsync(StateHasChanged);
                                                     await Task.Delay(1000);
                                                 }

                                                 _tokenSource.Token.ThrowIfCancellationRequested();
                                                 await LoadGamblingData();
                                                 await InvokeAsync(StateHasChanged);
                                             }
                                         });
    }

    public async Task LoadGamblingData()
    {
        try
        {
            _isUpdating = true;
            _firstLoad = false;
            await InvokeAsync(StateHasChanged);

            var currency = await CurrencyService.GetAll();
            if (currency is null || currency.Count == 0) return;
            _currency = currency;

            _filterValues.Currency = _filterValues.Currency is null
                                         ? _currency.First(c => c.Name.Equals("Divine Orb"))
                                         : _currency.FirstOrDefault(c => c.Id.Equals(_filterValues.Currency.Id));
            _filterValues.Currency ??= _currency.First(c => c.Name.Equals("Divine Orb"));


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
            _lastBackendUpdate = DateTime.Now;

            _isUpdating = false;
            await InvokeAsync(StateHasChanged);
            await JsRuntime.InvokeVoidAsync("addTooltips");
        }
    }

    private async void OnFilterValuesChanged(FilterValues filterValues)
    {
        _filterValues = filterValues;
        _currentGemPage = 0;
        _isOnLastPage = false;
        await LoadGamblingData();
    }

    private string LastUpdateText()
    {
        return _lastBackendUpdate == DateTime.MinValue
                   ? "Never"
                   : _lastBackendUpdate < DateTime.Now.AddMinutes(-1)
                       ? $"{PassedMinutesSinceUpdate()} Minute{(PassedMinutesSinceUpdate() == 1 ? "" : "s")} ago"
                       : "Just now";
    }

    private int PassedMinutesSinceUpdate() { return (int)DateTime.Now.Subtract(_lastBackendUpdate).TotalMinutes; }

    private async Task<bool> UpdateGems()
    {
        if (_isOnLastPage) return false;
        var gemPage = await GemService.GetAll(new PageRequest { PageSize = 20, PageNumber = _currentGemPage },
                                              _filterValues.ToQuery());
        if (gemPage is null) return false;
        _gems.AddRange(gemPage.Content);
        _currentGemPage = gemPage.CurrentPage;
        _isOnLastPage = gemPage.LastPage;
        return true;
    }

    #region Dispose

    private readonly CancellationTokenSource _tokenSource = new();

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        _tokenSource.Cancel();
        try
        {
            await _loadGamblingDataTask;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _loadGamblingDataTask.Dispose();
            _tokenSource.Dispose();
        }
    }

    #endregion

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