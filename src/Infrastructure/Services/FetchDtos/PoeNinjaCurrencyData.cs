using System.Text.Json.Serialization;
using Domain.Entity;

namespace Infrastructure.Services.FetchDtos;

public class PoeNinjaCurrencyData
{
    public long Id { get; set; }
    [JsonPropertyName("currencyTypeName")] public string Name { get; set; }
    public decimal ChaosEquivalent { get; set; }
    public string DetailsId { get; set; }
    public string? Icon { get; set; }

    public Currency ToCurrencyData()
    {
        return new Currency
               {
                   Id = DetailsId,
                   Name = Name,
                   ChaosEquivalent = ChaosEquivalent,
                   Icon = Icon
               };
    }
}