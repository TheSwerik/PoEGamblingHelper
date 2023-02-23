namespace Infrastructure.Services.FetchDtos;

public class GemPriceData
{
    public PoeNinjaGemData[] Lines { get; set; } = null!;
    public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
}