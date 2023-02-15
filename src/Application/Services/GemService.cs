using System.Diagnostics;
using System.Text.RegularExpressions;
using Domain.Entity.Gem;
using Domain.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public partial class GemService : IGemService
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GemService(IApplicationDbContext applicationDbContext) { _applicationDbContext = applicationDbContext; }

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
        if (page is null)
            return new Page<GemData> { Content = _applicationDbContext.GemData, CurrentPage = 0, LastPage = true };

        var pageSize = page.PageSize ?? 0;
        var skipSize = pageSize * page.PageNumber;
        var takeSize = page.PageSize ?? int.MaxValue;

        var allContentLength = await _applicationDbContext.GemData.CountAsync();
        return new Page<GemData>
               {
                   Content = _applicationDbContext.GemData.Skip(skipSize).Take(takeSize),
                   LastPage = skipSize + takeSize >= allContentLength,
                   CurrentPage = page.PageNumber
               };
    }

    private GemData[] FilterGemData(GemDataQuery query)
    {
        query.SearchText = SqlSanitizeRegex().Replace(query.SearchText, "").ToLowerInvariant();
        query.PricePerTryFrom ??= decimal.MinValue;
        query.PricePerTryTo ??= decimal.MinValue;

        return _applicationDbContext.GemData
                                    .Where(gemData => PreFilterGemData(gemData, query))
                                    .Include(gemData => gemData.Gems)
                                    .AsEnumerable()
                                    .Where(gemData => PostFilterGemData(gemData, query))
                                    .OrderBy(gemData => OrderGemData(gemData, query.Sort))
                                    .ToArray();
    }

    private static bool PreFilterGemData(GemData gemData, GemDataQuery query)
    {
        var isAlternateQuality = EF.Functions.Like(gemData.Name.ToLower(), "anomalous%")
                                 || EF.Functions.Like(gemData.Name.ToLower(), "divergent%")
                                 || EF.Functions.Like(gemData.Name.ToLower(), "phantasmal%");
        var isVaal = EF.Functions.Like(gemData.Name.ToLower(), "vaal%");
        var isExceptional = EF.Functions.Like(gemData.Name.ToLower(), "%enhance%")
                            || EF.Functions.Like(gemData.Name.ToLower(), "%empower%")
                            || EF.Functions.Like(gemData.Name.ToLower(), "%enlighten%");
        var isSupport = EF.Functions.Like(gemData.Name.ToLower(), "%support");
        var isGemTypeMatching = query.GemType switch
                                {
                                    GemType.All => true,
                                    GemType.Exceptional => isExceptional,
                                    GemType.Awakened => EF.Functions.Like(gemData.Name.ToLower(), "awakened%"),
                                    GemType.RegularSupport => isSupport,
                                    GemType.Skill => !isSupport,
                                    _ => throw new UnreachableException()
                                };

        return EF.Functions.Like(gemData.Name.ToLower(), $"%{query.SearchText}%")
               && (query.ShowAlternateQuality || !isAlternateQuality)
               && (query.ShowVaal || !isVaal)
               && isGemTypeMatching;
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