using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoEGamblingHelper.Application.Exception;
using PoEGamblingHelper.Domain.Entity;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.DataFetcher;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class TempleDataFetcher(ILogger<TempleDataFetcher> logger,
                               IDbContextFactory<ApplicationDbContext> applicationDbContextFactory,
                               IHttpClientFactory httpClientFactory) : IDataFetcher
{
    private readonly MediaTypeHeaderValue _jsonMediaTypeHeader = MediaTypeHeaderValue.Parse("application/json");

    private readonly string _templeQuery =
        File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/TempleQuery.json");

    public async Task Fetch(League league)
    {
        #region Fetch Temple IDs

        logger.LogDebug("Fetching Temple IDs...");

        TradeResults temples;
        var requestUri = $"{PoeToolUrls.PoeApiTradeUrl}/search/{league.Name}";
        using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
        {
            request.Content = new StringContent(_templeQuery, _jsonMediaTypeHeader);
            var result = await SendAsync(request);
            if (!result.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);

            temples = await result.Content.ReadFromJsonAsync<TradeResults>() ??
                      throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);
        }

        logger.LogDebug("Found {ResultLength} Temples IDs", temples.Result.Length);

        #endregion

        #region Fetch Temples

        var takeAmount = Math.Min(10, temples.Result.Length);
        var skipAmount = takeAmount == temples.Result.Length ? 0 : 2;
        var itemQuery = string.Join(",", temples.Result.Skip(skipAmount).Take(takeAmount));

        logger.LogDebug("Skipping {SkipAmount} and fetching {TakeAmount} Temples...", skipAmount, takeAmount);

        TradeEntryResult priceResults;
        requestUri = $"{PoeToolUrls.PoeApiTradeUrl}/fetch/{itemQuery}?query={temples.Id}";
        using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
        {
            request.Content = new StringContent(_templeQuery, _jsonMediaTypeHeader);
            var result = await SendAsync(request);
            if (!result.IsSuccessStatusCode) throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);

            priceResults = await result.Content.ReadFromJsonAsync<TradeEntryResult>()
                           ?? throw new ApiDownException(PoeToolUrls.PoeApiTradeUrl);
        }

        logger.LogDebug("Found {ResultLength} TemplePrices", priceResults.Result.Length);

        #endregion

        await using var applicationDbContext = await applicationDbContextFactory.CreateDbContextAsync();
        var chaosValues = priceResults.Result
                                      .Select(priceResult =>
                                                  priceResult.Listing
                                                             .Price
                                                             .ChaosAmount(applicationDbContext.Currency)
                                      )
                                      .ToArray();

        await applicationDbContext.TempleCost.ExecuteDeleteAsync(); // Delete every Temple Entry
        await applicationDbContext.TempleCost.AddAsync(new TempleCost { ChaosValue = chaosValues });
        await applicationDbContext.SaveChangesAsync();

        logger.LogInformation("Saved {PriceLength} TemplePrices", chaosValues.Length);
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("PoEGamblingHelper/1.0.0"));
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            return await httpClient.SendAsync(request);
        }
        catch (HttpRequestException e)
        {
            throw new ApiDownException(request.RequestUri!.Host, e.Message);
        }
    }
}