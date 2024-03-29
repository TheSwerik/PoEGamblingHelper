﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Application.Util;
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
        var (skipSize, takeSize) = page.ConvertToSizes();

        return new Page<GemData>
               {
                   Content = allFoundGems.Skip(skipSize).Take(takeSize),
                   LastPage = skipSize + takeSize >= allFoundGems.Length,
                   CurrentPage = page.PageNumber
               };
    }

    private async Task<Page<GemData>> GetAll(PageRequest page)
    {
        using var applicationDbContext = _applicationDbContextFactory.CreateDbContext();

        var allContentLength = await applicationDbContext.GemData.CountAsync();
        var (skipSize, takeSize) = page.ConvertToSizes();

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
        var templeCost = applicationDbContext.TempleCost
                                             .OrderByDescending(cost => cost.TimeStamp)
                                             .FirstOrDefault()
                                             ?.AverageChaosValue() ?? 0;
        return applicationDbContext.GemData
                                   .FromSqlRaw(PreFilterGemDataQuery(query))
                                   .Include(gemData => gemData.Gems)
                                   .AsEnumerable()
                                   .Where(gemData => PostFilterGemData(gemData, query, templeCost))
                                   .OrderBy(gemData => OrderGemData(gemData, query.Sort, templeCost))
                                   .ToArray();
    }

    private static string PreFilterGemDataQuery(GemDataQuery query)
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
        return gemData.RawCost() >= query.PricePerTryFrom
               && gemData.RawCost() <= query.PricePerTryTo
               && (!query.OnlyShowProfitable || gemData.AvgProfitPerTry(templeCost: averageTempleCost) > 0);
    }

    private static decimal OrderGemData(GemData gemData, Sort sort, decimal averageTempleCost)
    {
        return sort switch
               {
                   Sort.CostPerTryAsc => gemData.RawCost(),
                   Sort.CostPerTryDesc => -gemData.RawCost(),
                   Sort.AverageProfitPerTryAsc => gemData.AvgProfitPerTry(templeCost: averageTempleCost),
                   Sort.AverageProfitPerTryDesc => -gemData.AvgProfitPerTry(templeCost: averageTempleCost),
                   Sort.MaxProfitPerTryAsc => -gemData.Profit(ResultCase.Best, templeCost: averageTempleCost),
                   Sort.MaxProfitPerTryDesc => -gemData.Profit(ResultCase.Best, templeCost: averageTempleCost),
                   _ => (decimal)Random.Shared.NextDouble()
               };
    }

    [GeneratedRegex("[^a-z ]")]
    private static partial Regex SqlSanitizeRegex();
}