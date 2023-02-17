namespace Infrastructure.Services.FetchDtos;

public class TradeResults
{
    public string Id { get; set; } = null!;
    public int Complexity { get; set; }
    public string[] Result { get; set; } = null!;
    public int Total { get; set; }
}