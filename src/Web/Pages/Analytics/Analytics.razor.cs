using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using PoEGamblingHelper.Web.Services.Interfaces;

namespace PoEGamblingHelper.Web.Pages.Analytics;

public partial class Analytics
{
    public enum RangeSelect
    {
        Last30Days,
        LastYear,
        CurrentLeague,
        Custom
    }

    private ChartData _chartData = null!;
    private DateTime _endDate = DateTime.UtcNow;
    private LineChart _lineChart = null!;
    private bool _lineChartInitialized;
    private LineChartOptions _lineChartOptions = null!;
    private RangeSelect _selectedRange = RangeSelect.Last30Days;
    private DateTime _startDate = DateTime.UtcNow.AddDays(-30);

    [Inject] private IAnalyticsService AnalyticsService { get; set; } = null!;
    [Inject] private ILeagueService LeagueService { get; set; } = null!;

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            Console.WriteLne(2);
            if (_selectedRange == RangeSelect.Custom) _ = UpdateSelectedRange(_selectedRange);
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            if (_selectedRange == RangeSelect.Custom) _ = UpdateSelectedRange(_selectedRange);
        }
    }

    private async Task UpdateSelectedRange(RangeSelect range)
    {
        _selectedRange = range;
        DateTime start;
        var end = DateTime.UtcNow;

        switch (range)
        {
            case RangeSelect.Last30Days:
                start = DateTime.UtcNow.AddDays(-30);
                break;
            case RangeSelect.LastYear:
                start = DateTime.UtcNow.AddYears(-1);
                break;
            case RangeSelect.CurrentLeague:
                var league = await LeagueService.GetCurrent();
                if (league is null) throw new Exception("League not found");
                start = league.StartDate;
                break;
            case RangeSelect.Custom:
                if (_startDate > _endDate) return;
                start = _startDate;
                end = _endDate;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(range), range, null);
        }

        var analytics = await AnalyticsService.Get(start, end);
        if (analytics is null) throw new Exception("Analytics not found");
        var data = analytics.Select(a => new Data($"{a.Date.Day}.{a.Date.Month}.{a.Date.Year}", a.Views)).ToList();
        await CreateLineChart(data);
    }

    private async Task CreateLineChart(List<Data> data)
    {
        var colors = ColorUtility.CategoricalTwelveColors;

        var labels = data.Select(d => d.Label).ToList();

        var dataset = new LineChartDataset
        {
            Label = "Views per Day",
            Data = data.Select(d => (double?)d.Value).ToList(),
            BackgroundColor = colors[0],
            BorderColor = colors[0],
            BorderWidth = 2,
            HoverBorderWidth = 4,
            PointBackgroundColor = [colors[0]],
            PointRadius = [0],
            PointHoverRadius = [4]
        };

        _chartData = new ChartData
        {
            Labels = labels,
            Datasets = [dataset]
        };

        _lineChartOptions = new LineChartOptions
        {
            Responsive = true,
            Interaction = new Interaction { Mode = InteractionMode.Index }
        };

        if (_lineChartInitialized)
        {
            await _lineChart.UpdateAsync(_chartData, _lineChartOptions);
        }
        else
        {
            await _lineChart.InitializeAsync(_chartData, _lineChartOptions);
            _lineChartInitialized = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await UpdateSelectedRange(_selectedRange);
        await base.OnInitializedAsync();
    }

    private record Data(string Label, double Value);
}