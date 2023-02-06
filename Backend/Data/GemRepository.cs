using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.QueryParameters;

namespace Backend.Data;

public partial class GemDataRepository : Repository<GemData, Guid>, IGemDataRepository
{
    public GemDataRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

    public Page<GemData> GetAll(GemDataQuery? query, PageRequest page)
    {
        if (query is null) return GetAll(page, entities => entities.Include(gemData => gemData.Gems));
        query.SearchText = SQLSanitizeRegex().Replace(query.SearchText, "").ToLowerInvariant();
        var skipSize = page.PageNumber * page.PageSize ?? 0;
        var takeSize = page.PageSize ?? int.MaxValue;
        query.PricePerTryFrom ??= decimal.MinValue;
        query.PricePerTryTo ??= decimal.MaxValue;

        var allFoundGems = Entities
                           .Where(gemData => EF.Functions.Like(gemData.Name.ToLower(), $"%{query.SearchText}%")
                                             && ((query.GemType == GemType.Awakened &&
                                                  gemData.Name.StartsWith("Awakened"))
                                                 || (query.GemType == GemType.Exceptional &&
                                                     (gemData.Name.Contains("Enhance") ||
                                                      gemData.Name.Contains("Empower") ||
                                                      gemData.Name.Contains("Enlighten")))
                                                 || (query.GemType == GemType.Skill &&
                                                     !gemData.Name.EndsWith("Support"))
                                                 || (query.GemType == GemType.RegularSupport &&
                                                     gemData.Name.EndsWith("Support"))
                                                 || query.GemType == GemType.All)
                           )
                           .Include(gemData => gemData.Gems)
                           .AsEnumerable()
                           .Where(gemData => gemData.RawCost() >= query.PricePerTryFrom
                                             && gemData.RawCost() <= query.PricePerTryTo
                                             && (!query.OnlyShowProfitable || gemData.AvgProfitPerTry() > 0)
                           )
                           .OrderBy(gemData => query.Sort switch
                                               {
                                                   Sort.CostPerTryAsc => gemData.RawCost(),
                                                   Sort.CostPerTryDesc => -gemData.RawCost(),
                                                   Sort.AverageProfitPerTryAsc => gemData.AvgProfitPerTry(),
                                                   Sort.AverageProfitPerTryDesc => -gemData.AvgProfitPerTry(),
                                                   Sort.MaxProfitPerTryAsc => -gemData.Profit(ResultCase.Best),
                                                   Sort.MaxProfitPerTryDesc => -gemData.Profit(ResultCase.Best),
                                                   _ => (decimal)Random.Shared.NextDouble()
                                               })
                           .ToArray();

        return new Page<GemData>
               {
                   Content = allFoundGems.Skip(skipSize).Take(takeSize),
                   LastPage = skipSize + takeSize >= allFoundGems.Length,
                   CurrentPage = page.PageNumber
               };
    }

    [GeneratedRegex("[^a-z ]")] private static partial Regex SQLSanitizeRegex();
}