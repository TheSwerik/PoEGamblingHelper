using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace PoEGamblingHelper.Web.Shared.Components;

public partial class MathInput : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    private async Task OnValueChanged(ChangeEventArgs obj)
    {
        if (obj.Value is null) return;
        var value = obj.Value.ToString()!.Trim();
        if (!InputRegex().IsMatch(value)) return;
        Value = Calc(value).ToString(CultureInfo.InvariantCulture);
        await ValueChanged.InvokeAsync(Value);
    }

    private static decimal Calc(string expression)
    {
        var addition = expression.Split('+');
        if (addition.Length > 1) return addition.Aggregate(0m, (acc, val) => acc + Calc(val));

        var subtractions = expression.Split('-');
        if (subtractions.Length > 1) return subtractions.Skip(1).Aggregate(Calc(subtractions[0]), (acc, val) => acc - Calc(val));

        var multiplications = expression.Split('*');
        if (multiplications.Length > 1) return multiplications.Aggregate(1m, (acc, val) => acc * Calc(val));

        var division = expression.Split('/');
        if (division.Length > 1) return division.Skip(1).Aggregate(Calc(division[0]), (acc, val) => acc / Calc(val));

        return decimal.Parse(expression);
    }

    [GeneratedRegex(@"^([0-9]+(\.[0-9]+)? ?[\+\-\*\/]? ?)+$")]
    private static partial Regex InputRegex();
}