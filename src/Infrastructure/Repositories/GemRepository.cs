using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PoEGamblingHelper.Application.Extensions;
using PoEGamblingHelper.Application.QueryParameters;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Domain.Entity.Gem;
using PoEGamblingHelper.Infrastructure.Database;

namespace PoEGamblingHelper.Infrastructure.Repositories;

public partial class GemRepository(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    ITempleRepository templeRepository)
    : IGemRepository
{
    public Page<GemData> Search(GemDataQuery? query, PageRequest page)
    {
        return query is null
                   ? GetAll(page)
                   : GeneratePage(FilterGemData(query), page);
    }

    private Page<GemData> GetAll(PageRequest page)
    {
        using var context = dbContextFactory.CreateDbContext();
        return GeneratePage(context.GemData.ToArray(), page);
    }

    private GemData[] FilterGemData(GemDataQuery query)
    {
        query.SearchText = SqlSanitizeRegex().Replace(query.SearchText, "").ToLowerInvariant();
        query.PricePerTryFrom ??= decimal.MinValue;
        query.PricePerTryTo ??= decimal.MaxValue;
        var preFilterSqlQuery = PreFilterSqlQuery(query);
        var templeCost = templeRepository.GetCurrent(query.League).AverageChaosValue();

        using var applicationDbContext = dbContextFactory.CreateDbContext();
        return applicationDbContext.GemData
                                   .FromSqlRaw(preFilterSqlQuery)
                                   .Include(gemData => gemData.Gems.Where(g => g.League.Equals(query.League)))
                                   .AsEnumerable()
                                   .Where(gemData => PostFilterGemData(gemData, query, templeCost))
                                   .OrderBy(gemData => OrderGemData(gemData, query.Sort, templeCost))
                                   .ToArray();
    }

    private static string PreFilterSqlQuery(GemDataQuery query)
    {
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
                          AND ({query.ShowVaal} OR NOT {isVaal})
                          AND {isGemTypeMatching}
                """;
    }

    private static bool PostFilterGemData(GemData gemData, GemDataQuery query, decimal averageTempleCost)
    {
        var cost = gemData.RawCost();
        return cost >= query.PricePerTryFrom &&
               cost <= query.PricePerTryTo &&
               (!query.OnlyShowProfitable || gemData.AvgProfitPerTry(templeCost: averageTempleCost) > 0);
    }

    private static decimal OrderGemData(GemData gemData, Sort sort, decimal averageTempleCost)
    {
        return sort switch
        {
            Sort.CostPerTryAsc => gemData.RawCost(),
            Sort.CostPerTryDesc => -gemData.RawCost(),
            Sort.AverageProfitPerTryAsc => gemData.AvgProfitPerTry(templeCost: averageTempleCost),
            Sort.AverageProfitPerTryDesc => -gemData.AvgProfitPerTry(templeCost: averageTempleCost),
            Sort.MaxProfitPerTryAsc => gemData.Profit(ResultCase.Best, templeCost: averageTempleCost),
            Sort.MaxProfitPerTryDesc => -gemData.Profit(ResultCase.Best, templeCost: averageTempleCost),
            _ => 0m
        };
    }

    [GeneratedRegex("[^a-z ]")] private static partial Regex SqlSanitizeRegex();

    private static Page<GemData> GeneratePage(IReadOnlyCollection<GemData> gemData, PageRequest page)
    {
        var skipSize = page.PageSize * page.PageNumber;
        return new Page<GemData>
        {
            Content = gemData.Skip(skipSize).Take(page.PageSize).ToImmutableList(),
            LastPage = skipSize + page.PageSize >= gemData.Count,
            CurrentPage = page.PageNumber
        };
    }
}