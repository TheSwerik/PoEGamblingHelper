using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class CurrencyDataFetcher(
    ILogger<CurrencyDataFetcher> logger,
    IDbContextFactory<ApplicationDbContext> applicationDbContextFactory,
    IHttpClientFactory httpClientFactory)
    : IDataFetcher
{
    public async Task Fetch(string league)
    {
        var response = await GetAsync($"{PoeToolUrls.PoeNinjaCurrencyUrl}&league={league}");
        if (!response.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeNinjaCurrencyUrl);
        var currencyPriceData = await response.Content.ReadFromJsonAsync<CurrencyPriceData>();
        if (currencyPriceData is null) throw new ApiDownException(PoeToolUrls.PoeNinjaCurrencyUrl);
        logger.LogInformation("Got data from {Result} currency items", currencyPriceData.Lines.Length);

        foreach (var currencyData in currencyPriceData.Lines) // set icons
        {
            var currencyDetails = currencyPriceData.CurrencyDetails
                                                   .FirstOrDefault(currencyDetails =>
                                                                       currencyData.CurrencyTypeName.EqualsIgnoreCase(
                                                                           currencyDetails.Name));
            if (currencyDetails is not null) currencyData.Icon = currencyDetails.Icon;
        }

        await using var applicationDbContext = await applicationDbContextFactory.CreateDbContextAsync();

        var existingCurrency = applicationDbContext.Currency
                                                   .AsNoTracking()
                                                   .ToArray();

        var newPoeNinjaCurrencyData = currencyPriceData.Lines
                                                       .Where(currencyData =>
                                                                  !existingCurrency.Any(c => c.Id.Equals(currencyData.DetailsId) && c.League.Equals(league)))
                                                       .ToArray();
        await applicationDbContext.Currency.AddRangeAsync(
            newPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData(league)));
        logger.LogInformation("Added {Result} new Currency", newPoeNinjaCurrencyData.Length);

        var updatedPoeNinjaCurrencyData = currencyPriceData.Lines
                                                           .Where(gem => existingCurrency.Any(c => c.Id.Equals(gem.DetailsId) && c.League.Equals(league)))
                                                           .ToArray();
        applicationDbContext.Currency.UpdateRange(
            updatedPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData(league)));
        logger.LogInformation("Updated {Result} Currency", updatedPoeNinjaCurrencyData.Length);

        await applicationDbContext.SaveChangesAsync();

        #region Hardcoded Chaos Orb

        // not EqualsIgnoreCase because of EntityFramework
        var chaos = applicationDbContext.Currency
                                        .FirstOrDefault(currency => currency.Name.ToLower().Equals("chaos orb") && currency.League.Equals(league));
        if (chaos is not null) return;
        await applicationDbContext.Currency.AddAsync(new Currency
        {
            Name = "Chaos Orb",
            ChaosEquivalent = 1,
            Icon =
                "https://web.poecdn.com/image/Art/2DItems/Currency/CurrencyRerollRare.png",
            Id = "chaos-orb",
            League = league
        });
        logger.LogInformation("Saved Chaos Orb");

        #endregion

        await applicationDbContext.SaveChangesAsync();
    }

    private async Task<HttpResponseMessage> GetAsync(string url)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("PoEGamblingHelper/1.0.0"));
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            return await httpClient.GetAsync(url);
        }
        catch (HttpRequestException e)
        {
            throw new ApiDownException(url, e.Message);
        }
    }
}