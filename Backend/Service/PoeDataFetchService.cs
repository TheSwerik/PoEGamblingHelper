using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Backend.Exceptions;
using HtmlAgilityPack;
using Shared.Entity;
using Shared.Util;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Backend.Service;

public class PoeDataFetchService : Service, IPoeDataFetchService
{
    public const int PoeNinjaFetchMinutes = 5;
    private readonly HttpClient _client = new();
    private readonly IRepository<Currency, string> _currencyRepository;
    private readonly IGemDataRepository _gemDataRepository;
    private readonly IRepository<GemTradeData, long> _gemTradeDataRepository;
    private readonly IRepository<League, Guid> _leagueRepository;
    private readonly IPoeDataService _poeDataService;
    private readonly IRepository<TempleCost, Guid> _templeCostRepository;

    #region Helper Methods

    private static (int versionColumn, int nameColumn, int releaseColumn) GetIndexesFromTitleRow(HtmlNode titleRow)
    {
        var versionColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("version", StringComparison.InvariantCultureIgnoreCase)
        );
        if (versionColumn is null) throw new PoeDbCannotParseException("no version column");
        var versionColumnIndex = titleRow.ChildNodes.IndexOf(versionColumn);

        var nameColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("league", StringComparison.InvariantCultureIgnoreCase)
        );
        if (nameColumn is null) throw new PoeDbCannotParseException("no name column");
        var nameColumnIndex = titleRow.ChildNodes.IndexOf(nameColumn);

        var releaseColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("international", StringComparison.InvariantCultureIgnoreCase)
        );
        if (releaseColumn is null) throw new PoeDbCannotParseException("no release column");
        var releaseColumnIndex = titleRow.ChildNodes.IndexOf(releaseColumn);

        return (versionColumnIndex, nameColumnIndex, releaseColumnIndex);
    }

    #endregion

    #region HelperClasses

    private class CurrencyPriceData
    {
        public PoeNinjaCurrencyData[] Lines { get; set; } = null!;
        public PoeNinjaCurrencyDetails[] CurrencyDetails { get; set; } = null!;
        public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
    }

    private class GemPriceData
    {
        public PoeNinjaGemData[] Lines { get; set; } = null!;
        public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
    }

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

    public class PoeNinjaCurrencyDetails
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
    }

    public class PoeNinjaGemData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int GemLevel { get; set; }
        public int GemQuality { get; set; }
        public bool Corrupted { get; set; }
        public string DetailsId { get; set; }
        public decimal ChaosValue { get; set; }
        public decimal ExaltedValue { get; set; }
        public decimal DivineValue { get; set; }
        public int ListingCount { get; set; }

        public GemTradeData ToGemTradeData()
        {
            return new GemTradeData
                   {
                       Id = Id,
                       Name = Name,
                       GemLevel = GemLevel,
                       GemQuality = GemQuality,
                       Corrupted = Corrupted,
                       DetailsId = DetailsId,
                       ChaosValue = ChaosValue,
                       ExaltedValue = ExaltedValue,
                       DivineValue = DivineValue,
                       ListingCount = ListingCount
                   };
        }
    }

    private class TradeResults
    {
        public string Id { get; set; } = null!;
        public int Complexity { get; set; }
        public string[] Result { get; set; } = null!;
        public int Total { get; set; }
    }

    private class TradeEntryResult
    {
        public TradeEntry[] Result { get; set; } = null!;
    }

    private class TradeEntry
    {
        public string Id { get; set; } = null!;

        public TradeEntryListing Listing { get; set; } = null!;
    }

    private class TradeEntryListing
    {
        public TradeEntryListingPrice Price { get; set; } = null!;
    }

    private class TradeEntryListingPrice
    {
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;

        public decimal ChaosAmount(IRepository<Currency, string> currencyRepository)
        {
            var currency = currencyRepository.GetAll()
                                             .FirstOrDefault(c => c.Name.Equals(
                                                                 Currency + " orb",
                                                                 StringComparison.InvariantCultureIgnoreCase));
            var conversionValue = currency?.ChaosEquivalent ?? 0;
            return Amount * conversionValue;
        }
    }

    #endregion

    #region Con- and Destruction

    public PoeDataFetchService(ILogger<PoeDataFetchService> logger, IServiceScopeFactory factory) : base(
        logger, factory)
    {
        _gemDataRepository = Scope.ServiceProvider.GetRequiredService<IGemDataRepository>();
        _gemTradeDataRepository = Scope.ServiceProvider.GetRequiredService<IRepository<GemTradeData, long>>();
        _currencyRepository = Scope.ServiceProvider.GetRequiredService<IRepository<Currency, string>>();
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League, Guid>>();
        _templeCostRepository = Scope.ServiceProvider.GetRequiredService<IRepository<TempleCost, Guid>>();
        _poeDataService = Scope.ServiceProvider.GetRequiredService<IPoeDataService>();
        _client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("PoEGamblingHelper/1.0.0"));
        _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
    }

    public new void Dispose()
    {
        base.Dispose();
        _client.Dispose();
    }

    #endregion

    #region public methods

    public async Task GetCurrentLeague()
    {
        var web = new HtmlWeb();
        var doc = web.Load(PoeToolUrls.PoeDbUrl + "League#LeaguesList");
        if (doc is null) throw new PoeDbDownException();

        var tables = doc.DocumentNode.SelectNodes("//table");
        if (tables is null) throw new PoeDbCannotParseException("No tables found");
        var leaguesTable = tables.FirstOrDefault(n => n.HasChildNodes && n.InnerHtml.Contains("Weeks"));
        if (leaguesTable is null) throw new PoeDbCannotParseException("No tables found");

        var leagues = leaguesTable.SelectNodes(".//tr").Where(n => n.HasChildNodes).ToArray();
        if (leagues.Length == 0) throw new PoeDbCannotParseException("No rows found");
        var titleRow = leagues[0];

        var (versionColumn, nameColumn, releaseColumn) = GetIndexesFromTitleRow(titleRow);

        var yearRegex = new Regex("^\\d\\d\\d\\d$");
        var fullDateRegex = new Regex("^\\d\\d\\d\\d-\\d\\d-\\d\\d$");
        var nameExpansionRegex = new Regex("&lt;.+&gt;");
        foreach (var row in leagues.Skip(1).Where(row => row.HasChildNodes))
        {
            #region date

            var date = DateTime.MaxValue;
            if (yearRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = new DateTime(int.Parse(row.ChildNodes[releaseColumn].InnerText), 12, 31);
            else if (fullDateRegex.IsMatch(row.ChildNodes[releaseColumn].InnerText))
                date = DateTime.Parse(row.ChildNodes[releaseColumn].InnerText);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            #endregion

            var name = nameExpansionRegex.Replace(row.ChildNodes[nameColumn].InnerText, "").Trim();
            var version = row.ChildNodes[versionColumn].InnerText;

            var dbLeague = _leagueRepository
                           .GetAll()
                           .FirstOrDefault(
                               dbLeague => dbLeague.Version.Equals(version, StringComparison.InvariantCultureIgnoreCase)
                           );

            if (dbLeague is null)
            {
                var league = new League { Name = name, StartDate = date, Version = version };
                Logger.LogInformation("Saved League: {League}", league);
                await _leagueRepository.Save(league);
                continue;
            }

            dbLeague.Name = name;
            dbLeague.StartDate = date;
            dbLeague.Version = version;
            Logger.LogInformation("Updated League: {League}", dbLeague);
            await _leagueRepository.Update(dbLeague);
        }
    }

    public async Task GetPriceData()
    {
        var currentLeague = _poeDataService.GetCurrentLeague();
        if (currentLeague is null) return;
        try
        {
            await GetCurrencyData(currentLeague);
        }
        catch (PoeGamblingHelperException e)
        {
            Logger.LogError("{Exception}", e);
        }

        try
        {
            await GetTemplePriceData(currentLeague);
        }
        catch (PoeGamblingHelperException e)
        {
            Logger.LogError("{Exception}", e);
        }

        try
        {
            await GetGemPriceData(currentLeague);
        }
        catch (PoeGamblingHelperException e)
        {
            Logger.LogError("{Exception}", e);
        }
    }

    #region GetPriceData

    private async Task GetCurrencyData(League league)
    {
        const string currencyUrl = PoeToolUrls.PoeNinjaUrl + "/currencyoverview?type=Currency";
        var response = await _client.GetAsync(currencyUrl + $"&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new PoeNinjaDownException();
        var result = await response.Content.ReadFromJsonAsync<CurrencyPriceData>();
        if (result is null) throw new PoeNinjaDownException();
        Logger.LogInformation("Got data from {Result} currency items", result.Lines.Length);

        foreach (var poeNinjaCurrencyData in result.Lines)
        {
            var details =
                result.CurrencyDetails.FirstOrDefault(
                    details => poeNinjaCurrencyData.Name.EqualsIgnoreCase(details.Name));
            if (details is not null) poeNinjaCurrencyData.Icon = details.Icon;
        }


        var trackedCurrency = _currencyRepository.GetAll().Select(gem => gem.Id).ToArray();
        _currencyRepository.ClearTrackedEntities();


        var newPoeNinjaCurrencyData = result.Lines.Where(gem => !trackedCurrency.Contains(gem.DetailsId)).ToArray();
        await _currencyRepository.Save(newPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        Logger.LogInformation("Added {Result} new Currency", newPoeNinjaCurrencyData.Length);

        var updatedPoeNinjaCurrencyData = result.Lines.Where(gem => trackedCurrency.Contains(gem.DetailsId)).ToArray();
        await _currencyRepository.Update(
            updatedPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        Logger.LogInformation("Updated {Result} Currency", updatedPoeNinjaCurrencyData.Length);

        #region Chaos

        var chaos = _currencyRepository.GetAll()
                                       .FirstOrDefault(
                                           currency => currency.Name.Equals("Chaos Orb",
                                                                            StringComparison
                                                                                .InvariantCultureIgnoreCase));
        if (chaos is not null) return;
        await _currencyRepository.Save(new Currency
                                       {
                                           Name = "Chaos Orb",
                                           ChaosEquivalent = 1,
                                           Icon =
                                               "https://web.poecdn.com/image/Art/2DItems/Currency/CurrencyRerollRare.png",
                                           Id = "chaos-orb"
                                       });
        Logger.LogInformation("Saved Chaos Orb");

        #endregion
    }

    private async Task GetGemPriceData(League league)
    {
        const string gemUrl = PoeToolUrls.PoeNinjaUrl + "/itemoverview?type=SkillGem";
        var response = await _client.GetAsync(gemUrl + $"&league={league.Name}");
        if (!response.IsSuccessStatusCode) throw new PoeNinjaDownException();
        var result = await response.Content.ReadFromJsonAsync<GemPriceData>();
        if (result is null) throw new PoeNinjaDownException();
        Logger.LogInformation("Got data from {Result} gems", result.Lines.Length);

        #region GemTradeData

        var trackedGemTradeData = _gemTradeDataRepository.GetAll().Select(gem => gem.Id).ToArray();
        _gemTradeDataRepository.ClearTrackedEntities();

        var newPoeNinjaTradeData = result.Lines.Where(gem => !trackedGemTradeData.Contains(gem.Id)).ToArray();
        await _gemTradeDataRepository.Save(newPoeNinjaTradeData.Select(poeNinjaData => poeNinjaData.ToGemTradeData()));
        Logger.LogInformation("Added {Result} new GemTradeData", newPoeNinjaTradeData.Length);
        var updatedPoeNinjaTradeData = result.Lines.Where(gem => trackedGemTradeData.Contains(gem.Id)).ToArray();
        await _gemTradeDataRepository.Update(
            updatedPoeNinjaTradeData.Select(poeNinjaData => poeNinjaData.ToGemTradeData()));
        Logger.LogInformation("Updated {Result} GemTradeData", updatedPoeNinjaTradeData.Length);

        #endregion

        #region GemData

        var trackedGemData = _gemDataRepository.GetAll().ToArray();
        var trackedGemDataNames = trackedGemData.Select(gem => gem.Name.ToLowerInvariant().Trim()).ToArray();
        _gemDataRepository.ClearTrackedEntities();

        _gemTradeDataRepository.ClearTrackedEntities();
        var allGemTradeData = _gemTradeDataRepository.GetAll().ToArray();
        var groupedPoeNinjaData = result.Lines.GroupBy(poeNinjaGemData => poeNinjaGemData.Name).ToArray();
        var newPoeNinjaGemData = groupedPoeNinjaData
                                 .Where(group => !trackedGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                                 .ToArray();
        await _gemDataRepository.Save(newPoeNinjaGemData.Select(group => new GemData
                                                                         {
                                                                             Name = group.Key,
                                                                             Icon = group.First().Icon,
                                                                             Gems = allGemTradeData
                                                                                 .Where(tradeData =>
                                                                                     tradeData.Name
                                                                                         .EqualsIgnoreCase(
                                                                                             group.Key))
                                                                                 .ToList()
                                                                         }));
        Logger.LogInformation("Added {Result} GemData", newPoeNinjaGemData.Length);
        var updatedPoeNinjaGemData = groupedPoeNinjaData
                                     .Where(group => trackedGemDataNames.Contains(group.Key.ToLowerInvariant().Trim()))
                                     .ToArray();
        await _gemDataRepository.Update(updatedPoeNinjaGemData.Select(group => new GemData
                                                                               {
                                                                                   Id = trackedGemData
                                                                                       .First(gem => gem.Name
                                                                                           .EqualsIgnoreCase(
                                                                                               group.Key)).Id,
                                                                                   Name = group.Key,
                                                                                   Icon = group.First().Icon,
                                                                                   Gems = allGemTradeData
                                                                                       .Where(tradeData =>
                                                                                           tradeData.Name
                                                                                               .EqualsIgnoreCase(
                                                                                                   group.Key))
                                                                                       .ToList()
                                                                               }));
        Logger.LogInformation("Updated {Result} GemData", updatedPoeNinjaGemData.Length);

        #endregion
    }

    private async Task GetTemplePriceData(League league)
    {
        const string tradeUrl = PoeToolUrls.PoeApiUrl + "/trade";

        #region GetItems

        const string templeQuery =
            """{"query":{"status":{"option":"online"},"type":"Chronicle of Atzoatl","stats":[{"type":"and","filters":[{"id":"pseudo.pseudo_temple_gem_room_3","disabled":false,"value":{"option":1}}],"disabled":false}]},"sort":{"price":"asc"}}""";
        var request = new HttpRequestMessage(HttpMethod.Post, $"{tradeUrl}/search/{league.Name}");

        Logger.LogDebug("Fetching Temples...");

        request.Content = new StringContent(templeQuery, MediaTypeHeaderValue.Parse("application/json"));
        var result = await _client.SendAsync(request);
        if (!result.IsSuccessStatusCode) throw new PoeTradeDownException();
        var temples = await result.Content.ReadFromJsonAsync<TradeResults>();
        if (temples is null) throw new PoeTradeDownException();

        Logger.LogDebug("Found {ResultLength} Temples", temples.Result.Length);

        #endregion

        #region FetchItems

        var takeAmount = Math.Min(10, temples.Result.Length);
        var skipAmount = takeAmount == temples.Result.Length ? 0 : 2;
        var itemQuery = string.Join(",", temples.Result.Skip(skipAmount).Take(takeAmount));

        Logger.LogDebug("Skipping {SkipAmount} and fetching {TakeAmount} Temples...", skipAmount, takeAmount);

        request = new HttpRequestMessage(HttpMethod.Get, $"{tradeUrl}/fetch/{itemQuery}?query={temples.Id}");
        request.Content = new StringContent(templeQuery, MediaTypeHeaderValue.Parse("application/json"));
        result = await _client.SendAsync(request);
        var priceResults = await result.Content.ReadFromJsonAsync<TradeEntryResult>();
        if (priceResults is null) throw new PoeTradeDownException();

        Logger.LogDebug("Found {ResultLength} TemplePrices", priceResults.Result.Length);
        var templeCost = new TempleCost
                         {
                             ChaosValue = priceResults.Result
                                                      .Select(priceResult =>
                                                                  priceResult.Listing.Price.ChaosAmount(
                                                                      _currencyRepository))
                                                      .ToArray()
                         };

        var existingTemples = _templeCostRepository.GetAll().Select(temple => temple.Id).ToArray();
        _templeCostRepository.ClearTrackedEntities();
        foreach (var id in existingTemples) await _templeCostRepository.Delete(id);
        await _templeCostRepository.Save(templeCost);
        Logger.LogInformation("Saved {PriceLength} TemplePrices", templeCost.ChaosValue.Length);

        #endregion
    }

    #endregion

    #endregion
}