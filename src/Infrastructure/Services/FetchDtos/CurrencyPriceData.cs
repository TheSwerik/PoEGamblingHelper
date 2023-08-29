namespace PoEGamblingHelper.Infrastructure.Services.FetchDtos;

public class CurrencyPriceData
{
    public PoeNinjaCurrencyData[] Lines { get; set; } = null!;
    public PoeNinjaCurrencyDetails[] CurrencyDetails { get; set; } = null!;
    public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
}