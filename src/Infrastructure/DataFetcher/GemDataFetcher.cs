using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;
using PoEGamblingHelper.Infrastructure.Extensions;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class GemDataFetcher(ILogger<GemDataFetcher> logger,
                            IDbContextFactory<ApplicationDbContext> applicationDbContextFactory,
                            IHttpClientFactory httpClientFactory) : IDataFetcher
{
    public async Task Fetch(League league)
    {
        var response = await GetAsync(PoeToolUrls.PoeNinjaGemUrl + $"&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeNinjaGemUrl);
        var gemPriceData = await response.Content.ReadFromJsonAsync<GemPriceData>();
        if (gemPriceData is null) throw new ApiDownException(PoeToolUrls.PoeNinjaGemUrl);
        logger.LogInformation("Got data from {Result} gems", gemPriceData.Lines.Length);

        await FetchGemTradeData(gemPriceData);
        await FetchGemData(gemPriceData);
    }

    private async Task FetchGemData(GemPriceData gemPriceData)
    {
        await using var applicationDbContext = await applicationDbContextFactory.CreateDbContextAsync();

        var existingGemData = applicationDbContext.GemData
                                                  .AsNoTracking()
                                                  .ToArray();
        var existingGemDataNames = existingGemData.Select(gem => gem.Name.ToLowerInvariant().Trim())
                                                  .ToArray();

        var allGemTradeData = applicationDbContext.GemTradeData.ToArray();
        var groupedPoeNinjaData = gemPriceData.Lines.GroupBy(priceData => priceData.Name).ToArray();

        // Add
        var newGemData = groupedPoeNinjaData
                         .Where(group => !existingGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                         .ToArray();
        await applicationDbContext.GemData.AddRangeAsync(
            newGemData.Select(group => group.ToGemData(allGemTradeData)));
        logger.LogInformation("Added {Result} GemData", newGemData.Length);

        // Update
        var updatedGemData = groupedPoeNinjaData
                             .Where(group => existingGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                             .ToArray();
        applicationDbContext.GemData.UpdateRange(
            updatedGemData.Select(group => group.ToGemData(allGemTradeData, existingGemData))
        );
        logger.LogInformation("Updated {Result} GemData", updatedGemData.Length);

        await applicationDbContext.SaveChangesAsync();
    }

    private async Task FetchGemTradeData(GemPriceData gemPriceData)
    {
        await using var applicationDbContext = await applicationDbContextFactory.CreateDbContextAsync();

        var existingGemTradeData = applicationDbContext.GemTradeData
                                                       .AsNoTracking()
                                                       .Select(gemTradeData => gemTradeData.Id)
                                                       .ToArray();

        var newGemTradeData = gemPriceData.Lines
                                          .Where(gem => !existingGemTradeData.Contains(gem.Id))
                                          .ToArray();

        // Add
        await applicationDbContext.GemTradeData.AddRangeAsync(
            newGemTradeData.Select(gemTradeData => gemTradeData.ToGemTradeData()));
        logger.LogInformation("Added {Result} new GemTradeData", newGemTradeData.Length);

        // Update
        var updatedGemTradeData = gemPriceData.Lines
                                              .Where(gem => existingGemTradeData.Contains(gem.Id))
                                              .ToArray();
        applicationDbContext.GemTradeData.UpdateRange(
            updatedGemTradeData.Select(gemTradeData => gemTradeData.ToGemTradeData()));
        logger.LogInformation("Updated {Result} GemTradeData", updatedGemTradeData.Length);

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