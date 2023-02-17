using System.Diagnostics;
using System.Text.RegularExpressions;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public partial class GemService : IGemService
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;

    public GemService(IApplicationDbContextFactory applicationDbContextFactory)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
    }

    public async Task<Page<GemData>> GetAll(GemDataQuery? query, PageRequest page)
    {
        if (query is null) return await GetAll(page);

        var allFoundGems = FilterGemData(query);
        var skipSize = page.PageNumber * page.PageSize ?? 0;
        var takeSize = page.PageSize ?? int.MaxValue;

        return new Page<GemData>
               {
                   Content = allFoundGems.Skip(skipSize).Take(takeSize),
                   LastPage = skipSize + takeSize >= allFoundGems.Length,
                   CurrentPage = page.PageNumber
               };
    }

    private async Task<Page<GemData>> GetAll(PageRequest? page)
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        if (page is null)
            return new Page<GemData> { Content = applicationDbContext.GemData, CurrentPage = 0, LastPage = true };

        var pageSize = page.PageSize ?? 0;
        var skipSize = pageSize * page.PageNumber;
        var takeSize = page.PageSize ?? int.MaxValue;

        var allContentLength = await applicationDbContext.GemData.CountAsync();
        return new Page<GemData>
               {
                   Content = applicationDbContext.GemData.Skip(skipSize).Take(takeSize),
                   LastPage = skipSize + takeSize >= allContentLength,
                   CurrentPage = page.PageNumber
               };
    }

    private GemData[] FilterGemData(GemDataQuery query)
    {
        query.SearchText = SqlSanitizeRegex().Replace(query.SearchText, "").ToLowerInvariant();
        query.PricePerTryFrom ??= decimal.MinValue;
        query.PricePerTryTo ??= decimal.MaxValue;

        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();
        return applicationDbContext.GemData
                                   .FromSqlRaw(PreFilterGemDataQuery(query))
                                   .Include(gemData => gemData.Gems)
                                   .AsEnumerable()
                                   .Where(gemData => PostFilterGemData(gemData, query))
                                   .OrderBy(gemData => OrderGemData(gemData, query.Sort))
                                   .ToArray();
    }

    private static string PreFilterGemDataQuery(GemDataQuery query)
    {
        const string isAlternateQuality = """
               (LOWER("Name") LIKE 'anomalous%'
               OR LOWER("Name") LIKE 'divergent%'
               OR LOWER("Name") LIKE 'phantasmal%')
        """;
        const string isVaal = """LOWER("Name") LIKE 'vaal%' """;

        const string isExceptional = """
                (LOWER("Name") LIKE '%enhance%'
                OR LOWER("Name") LIKE '%empower%'
                OR LOWER("Name") LIKE '%enlighten%')
        """;
        const string isAwakened = """LOWER("Name") LIKE 'awakened%' """;
        const string isSupport = """LOWER("Name") LIKE '%support' """;
        var isGemTypeMatching = query.GemType switch
                                {
                                    GemType.All => true.ToString(),
                                    GemType.Exceptional => isExceptional,
                                    GemType.Awakened => isAwakened,
                                    GemType.RegularSupport => isSupport,
                                    GemType.Skill => $"NOT {isSupport}",
                                    _ => throw new UnreachableException()
                                };

        return $"""
            SELECT * FROM "GemData"
                WHERE LOWER("Name") LIKE '%{query.SearchText}%'
                  AND {query.ShowAlternateQuality} OR NOT {isAlternateQuality}
                  AND {query.ShowVaal} OR NOT {isVaal}
                  AND {isGemTypeMatching}
        """;
    }

    private static bool PostFilterGemData(GemData gemData, GemDataQuery query)
    {
        return gemData.RawCost() >= query.PricePerTryFrom
               && gemData.RawCost() <= query.PricePerTryTo
               && (!query.OnlyShowProfitable || gemData.AvgProfitPerTry() > 0);
    }

    private static decimal OrderGemData(GemData gemData, Sort sort)
    {
        return sort switch
               {
                   Sort.CostPerTryAsc => gemData.RawCost(),
                   Sort.CostPerTryDesc => -gemData.RawCost(),
                   Sort.AverageProfitPerTryAsc => gemData.AvgProfitPerTry(),
                   Sort.AverageProfitPerTryDesc => -gemData.AvgProfitPerTry(),
                   Sort.MaxProfitPerTryAsc => -gemData.Profit(ResultCase.Best),
                   Sort.MaxProfitPerTryDesc => -gemData.Profit(ResultCase.Best),
                   _ => (decimal)Random.Shared.NextDouble()
               };
    }

    [GeneratedRegex("[^a-z ]")] private static partial Regex SqlSanitizeRegex();
}