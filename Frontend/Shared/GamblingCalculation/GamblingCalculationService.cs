using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace PoEGamblingHelper2;

public class GamblingCalculationService
{
    private readonly HttpClient _httpClient;

    public GamblingCalculationService(HttpClient? httpClient)
    {
        _httpClient = httpClient ?? throw new Exception("HTTP CLIENT IS NULL");
        _httpClient.MaxResponseContentBufferSize = 2147483647;
    }

    public async Task<List<GamblingData>> GetCalculatedChances()
    {
        var lines = (await _httpClient.GetStringAsync("default-data/GemsToSearch.csv")).Split("\n");
        var gemsToSearch = lines.Skip(1)
                                .Where(line => !line.StartsWith('#'))
                                .Select(line => line.Split(','))
                                .Select(values => new GemToSearch { Name = values[0], MaxLevel = int.Parse(values[1]) })
                                .ToList();

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri("https://poe.ninja/api/data/itemoverview?league=Sentinel&type=SkillGem");
        // request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
        request.SetBrowserRequestMode(BrowserRequestMode.Cors);
        request.SetBrowserRequestCache(BrowserRequestCache.NoStore);
        request.Headers.Add("Access-Control-Allow-Origin", "");
        var result = await _httpClient.SendAsync(request);
        Console.WriteLine(result);
        Console.WriteLine(result.Content);
        Console.WriteLine(result.Content.Headers);
        Console.WriteLine(await result.Content.ReadAsByteArrayAsync());
        Console.WriteLine(await result.Content.ReadAsStringAsync());
        var data = result.Content.ReadFromJsonAsync<Data>().Result;
        if (data is null) Environment.Exit(1);


        Console.WriteLine("Chaos per Ex?");
        var exWorth = 175;

        Console.WriteLine("Average Temple Cost?");
        var templeCost = 100m / exWorth;

        var gamblingData = gemsToSearch.Select(gem => new GamblingData
                                                      {
                                                          Name = gem.Name,
                                                          Raw = data.Lines.FirstOrDefault(
                                                              i => i.Name.Equals(gem.Name) &&
                                                                   i.GemLevel == gem.MaxLevel - 1 &&
                                                                   !i.Corrupted),
                                                          BestCase = data.Lines.FirstOrDefault(
                                                              i => i.Name.Equals(gem.Name) &&
                                                                   i.GemLevel == gem.MaxLevel),
                                                          MediumCase = data.Lines.FirstOrDefault(
                                                              i => i.Name.Equals(gem.Name) &&
                                                                   i.GemLevel == gem.MaxLevel - 1 &&
                                                                   i.Corrupted),
                                                          WorstCase = data.Lines.FirstOrDefault(
                                                              i => i.Name.Equals(gem.Name) &&
                                                                   i.GemLevel == gem.MaxLevel - 2 &&
                                                                   i.Corrupted)
                                                      })
                                       .ToList();

        gamblingData.Sort((g1, g2) => decimal.Compare(g2.ProbablyProfitPerTry(templeCost),
                                                      g1.ProbablyProfitPerTry(templeCost)));
        Console.WriteLine("\n");
        foreach (var gamblingData1 in gamblingData)
        {
            Console.WriteLine($"{gamblingData1.Name}");
            Console.WriteLine($"Raw: {gamblingData1.Raw}");
            Console.WriteLine($"BestCase: {gamblingData1.BestCase}");
            Console.WriteLine($"MediumCase: {gamblingData1.MediumCase}");
            Console.WriteLine($"WorstCase: {gamblingData1.WorstCase}");
            Console.WriteLine($"Cost per Try: {gamblingData1.CostPerTry(templeCost)}");
            Console.WriteLine($"Min Profit Per Try: {gamblingData1.MinProfitPerTry(templeCost)}");
            Console.WriteLine($"Max Profit Per Try: {gamblingData1.MaxProfitPerTry(templeCost)}");
            Console.WriteLine($"Probable Profit Per Try: {gamblingData1.ProbablyProfitPerTry(templeCost)}");
            Console.WriteLine("\n");
        }

        Console.WriteLine("Buy Temples here: https://www.pathofexile.com/trade/search/Sentinel/LnmylZ5Tn");

        return gamblingData;
    }
}