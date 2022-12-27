using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Model;

namespace Backend.Service;

public class PoeDataFetchService : Service, IPoeDataFetchService
{
    private readonly HttpClient _client = new();
    private readonly IRepository<Currency, string> _currencyRepository;
    private readonly IRepository<GemData, Guid> _gemDataRepository;
    private readonly IRepository<GemTradeData, long> _gemTradeDataRepository;
    private readonly IRepository<League, Guid> _leagueRepository;
    private readonly IPoeDataService _poeDataService;

    #region Helper Methods

    private static (int versionColumn, int nameColumn, int releaseColumn) GetIndexesFromTitleRow(HtmlNode titleRow)
    {
        var versionColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("version", StringComparison.InvariantCultureIgnoreCase)
        );
        if (versionColumn is null) throw new NullReferenceException("no version column");
        var versionColumnIndex = titleRow.ChildNodes.IndexOf(versionColumn);

        var nameColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("league", StringComparison.InvariantCultureIgnoreCase)
        );
        if (nameColumn is null) throw new NullReferenceException("no name column");
        var nameColumnIndex = titleRow.ChildNodes.IndexOf(nameColumn);

        var releaseColumn = titleRow.ChildNodes.First(
            td => td.InnerText.Equals("international", StringComparison.InvariantCultureIgnoreCase)
        );
        if (releaseColumn is null) throw new NullReferenceException("no release column");
        var releaseColumnIndex = titleRow.ChildNodes.IndexOf(releaseColumn);

        return (versionColumnIndex, nameColumnIndex, releaseColumnIndex);
    }

    #endregion

    private class CurrencyPriceData
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public PoeNinjaCurrencyData[] Lines { get; set; } = null!;
        public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
    }

    private class GemPriceData
    {
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public PoeNinjaGemData[] Lines { get; set; } = null!;
        public override string ToString() { return string.Join(", ", Lines.AsEnumerable()); }
    }

    public class PoeNinjaCurrencyData
    {
        public long Id { get; set; }
        [JsonPropertyName("currencyTypeName")] public string Name { get; set; }
        public decimal ChaosEquivalent { get; set; }
        public string DetailsId { get; set; }

        public Currency ToCurrencyData()
        {
            return new Currency
                   {
                       Id = DetailsId,
                       Name = Name,
                       ChaosEquivalent = ChaosEquivalent,
                   };
        }
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

    #region Con- and Destruction

    public PoeDataFetchService(ILogger<PoeDataFetchService> logger, IServiceScopeFactory factory) : base(
        logger, factory)
    {
        _gemDataRepository = Scope.ServiceProvider.GetRequiredService<IRepository<GemData, Guid>>();
        _gemTradeDataRepository = Scope.ServiceProvider.GetRequiredService<IRepository<GemTradeData, long>>();
        _currencyRepository = Scope.ServiceProvider.GetRequiredService<IRepository<Currency, string>>();
        _leagueRepository = Scope.ServiceProvider.GetRequiredService<IRepository<League, Guid>>();
        _poeDataService = Scope.ServiceProvider.GetRequiredService<IPoeDataService>();
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
        if (doc is null) throw new NullReferenceException("PoeDB is down");

        var leaguesTable = doc.DocumentNode.SelectNodes("//table").First(n => n.HasChildNodes);
        if (leaguesTable is null) throw new NullReferenceException("No tables found");

        var leagues = leaguesTable.SelectNodes(".//tr").Where(n => n.HasChildNodes).ToArray();
        if (leagues.Length == 0) throw new NullReferenceException("No rows found");
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
        var currentLeague = await _poeDataService.GetCurrentLeague();
        await GetCurrencyData(currentLeague);
        await GetGemPriceData(currentLeague);

        var div = await _currencyRepository.Get("divine-orb");
        var gem = await _gemTradeDataRepository.Get(7064);
        Console.WriteLine("Divine ChaosEquivalent:\t\t\t" + div.ChaosEquivalent);
        Console.WriteLine("Gem ChaosValue:\t\t\t\t" + gem.ChaosValue);
        Console.WriteLine("Gem DivineValue:\t\t\t" + gem.DivineValue);
        Console.WriteLine("Gem DivineValue*Divine.ChaosEquivalent:\t" + gem.DivineValue * div.ChaosEquivalent);
        Console.WriteLine("Gem ChaosValue/Divine.ChaosEquivalent:\t" + gem.ChaosValue / div.ChaosEquivalent);
    }

    public async Task GetCurrencyData(League league)
    {
        const string currencyUrl = PoeToolUrls.PoeNinjaUrl + "/currencyoverview?type=Currency";
        var result = await _client.GetFromJsonAsync<CurrencyPriceData>(currencyUrl + $"&league={league.Name}");
        if (result is null) throw new NullReferenceException();
        Logger.LogInformation("Got data from {Result} currency items", result.Lines.Length);

        var trackedCurrency = _currencyRepository.GetAll().Select(gem => gem.Id).ToArray();
        _currencyRepository.ClearTrackedEntities();


        var newPoeNinjaCurrencyData = result.Lines.Where(gem => !trackedCurrency.Contains(gem.DetailsId)).ToArray();
        await _currencyRepository.Save(newPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        Logger.LogInformation("Added {Result} new Currency", newPoeNinjaCurrencyData.Length);

        var updatedPoeNinjaCurrencyData = result.Lines.Where(gem => trackedCurrency.Contains(gem.DetailsId)).ToArray();
        await _currencyRepository.Update(
            updatedPoeNinjaCurrencyData.Select(poeNinjaData => poeNinjaData.ToCurrencyData()));
        Logger.LogInformation("Updated {Result} Currency", updatedPoeNinjaCurrencyData.Length);
    }

    public async Task GetGemPriceData(League league)
    {
        const string gemUrl = PoeToolUrls.PoeNinjaUrl + "/itemoverview?type=SkillGem";
        var result = await _client.GetFromJsonAsync<GemPriceData>(gemUrl + $"&league={league.Name}");
        if (result is null) throw new NullReferenceException();
        Logger.LogInformation("Got data from {Result} gems", result.Lines.Length);

        var trackedGemData = _gemDataRepository.GetAll().Select(gem => gem.Name.ToLowerInvariant()).ToArray();
        var trackedGemTradeData = _gemTradeDataRepository.GetAll().Select(gem => gem.Id).ToArray();
        _gemDataRepository.ClearTrackedEntities();
        _gemTradeDataRepository.ClearTrackedEntities();

        #region GemTradeData

        var newPoeNinjaTradeData = result.Lines.Where(gem => !trackedGemTradeData.Contains(gem.Id)).ToArray();
        await _gemTradeDataRepository.Save(newPoeNinjaTradeData.Select(poeNinjaData => poeNinjaData.ToGemTradeData()));
        Logger.LogInformation("Added {Result} new GemTradeData", newPoeNinjaTradeData.Length);
        var updatedPoeNinjaTradeData = result.Lines.Where(gem => trackedGemTradeData.Contains(gem.Id)).ToArray();
        await _gemTradeDataRepository.Update(
            updatedPoeNinjaTradeData.Select(poeNinjaData => poeNinjaData.ToGemTradeData()));
        Logger.LogInformation("Updated {Result} GemTradeData", updatedPoeNinjaTradeData.Length);

        #endregion

        #region GemData

        _gemTradeDataRepository.ClearTrackedEntities();
        var allGemTradeData = _gemTradeDataRepository.GetAll().ToArray();
        var groupedPoeNinjaData = result.Lines.GroupBy(poeNinjaGemData => poeNinjaGemData.Name).ToArray();
        var newPoeNinjaGemData = groupedPoeNinjaData
                                 .Where(group => !trackedGemData.Contains(group.Key.ToLowerInvariant())).ToArray();
        await _gemDataRepository.Save(newPoeNinjaGemData.Select(group => new GemData
                                                                         {
                                                                             Name = group.Key,
                                                                             Icon = group.First().Icon,
                                                                             Gems = allGemTradeData
                                                                                 .Where(tradeData =>
                                                                                     tradeData.Name.Equals(
                                                                                         group.Key,
                                                                                         StringComparison
                                                                                             .InvariantCultureIgnoreCase))
                                                                                 .ToList()
                                                                         }));
        Logger.LogInformation("Added {Result} GemData", newPoeNinjaGemData.Length);
        var updatedPoeNinjaGemData = groupedPoeNinjaData
                                     .Where(group => trackedGemData.Contains(group.Key.ToLowerInvariant())).ToArray();
        await _gemDataRepository.Update(updatedPoeNinjaGemData.Select(group => new GemData
                                                                               {
                                                                                   Name = group.Key,
                                                                                   Icon = group.First().Icon,
                                                                                   Gems = allGemTradeData
                                                                                       .Where(tradeData =>
                                                                                           tradeData.Name
                                                                                               .Equals(
                                                                                                   group.Key,
                                                                                                   StringComparison
                                                                                                       .InvariantCultureIgnoreCase))
                                                                                       .ToList()
                                                                               }));
        Logger.LogInformation("Updated {Result} GemData", updatedPoeNinjaGemData.Length);

        #endregion
    }

    #endregion
}