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
    private LineChart _lineChart = null!;
    private LineChartOptions _lineChartOptions = null!;
    private RangeSelect _selectedRange = RangeSelect.Last30Days;

    [Inject] private IAnalyticsService AnalyticsService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var analytics = await AnalyticsService.GetAll();
        if (analytics is null) throw new Exception("Analytics not found");

        var last30Days = new DateTime[30];
        for (var i = 0; i < last30Days.Length; i++) last30Days[^(i + 1)] = DateTime.Today.AddDays(-i);


        var colors = ColorUtility.CategoricalTwelveColors;

        var labels = last30Days.Select(d => $"{d.Day}.{d.Month}.{d.Year}").ToList();

        var dataset = new LineChartDataset
        {
            Label = "Views per Day",
            Data = analytics.Take(30).Select(a => (double?)a.Views).ToList(),
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

        await _lineChart.InitializeAsync(_chartData, _lineChartOptions);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // if (firstRender) await _lineChart.InitializeAsync(_chartData, _lineChartOptions);
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateSelectedRange(RangeSelect range)
    {
        _selectedRange = range;
        switch (range)
        {
            case RangeSelect.Last30Days:
                Console.WriteLine(1);
                await CreateLineChart(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
                break;
            case RangeSelect.LastYear:
                await CreateLineChart(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);
                break;
            case RangeSelect.CurrentLeague:
                //TODO
                await CreateLineChart(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
                break;
            case RangeSelect.Custom:
                //TODO
                await CreateLineChart(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(range), range, null);
        }
    }

    private async Task CreateLineChart(DateTime start, DateTime end)
    {
        var analytics = await AnalyticsService.Get(start, end);
        if (analytics is null) throw new Exception("Analytics not found");

        var colors = ColorUtility.CategoricalTwelveColors;

        var labels = analytics.Select(a => $"{a.Date.Day}.{a.Date.Month}.{a.Date.Year}").ToList();

        var dataset = new LineChartDataset
        {
            Label = "Views per Day",
            Data = analytics.Take(30).Select(a => (double?)a.Views).ToList(),
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

        await _lineChart.InitializeAsync(_chartData, _lineChartOptions);
    }
}