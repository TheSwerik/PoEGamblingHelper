using Microsoft.AspNetCore.Components;

namespace PoEGamblingHelper.Web.Shared.Components;

public partial class MathInput : ComponentBase
{
    public string Value { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine(Calc("123.123 + 231232.32 / 132123 * 1234 * 432 - 123 * 123 + 13 / 54"));
    }

    private void Calculate(ChangeEventArgs obj)
    {
        var multiplications = Value.Split('*');
    }

    private static double Calc(string expression)
    {
        var addition = expression.Split('+');
        if (addition.Length > 1) return addition.Aggregate(0.0, (acc, val) => acc + Calc(val));

        var subtractions = expression.Split('-');
        if (subtractions.Length > 1) return subtractions.Skip(1).Aggregate(Calc(subtractions[0]), (acc, val) => acc - Calc(val));

        var multiplications = expression.Split('*');
        if (multiplications.Length > 1) return multiplications.Aggregate(1.0, (acc, val) => acc * Calc(val));

        var division = expression.Split('/');
        if (division.Length > 1) return division.Skip(1).Aggregate(Calc(division[0]), (acc, val) => acc / Calc(val));

        return double.Parse(expression);
    }
}