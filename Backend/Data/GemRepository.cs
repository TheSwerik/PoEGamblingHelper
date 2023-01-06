using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.QueryParameters;

namespace Backend.Data;

public class GemDataRepository : Repository<GemData, Guid>, IGemDataRepository
{
    public GemDataRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

    public IEnumerable<GemData> GetAll(GemDataQuery? query, Page? page)
    {
        if (query is null) return GetAll(page, entities => entities.Include(gemData => gemData.Gems));
        var skipSize = page?.PageNumber * page?.PageSize ?? 0;
        var takeSize = page?.PageSize ?? int.MaxValue;
        var stopwatch = Stopwatch.StartNew();
        var result = Entities
                     .Where(gemData => gemData.Name.Contains(query.SearchText,
                                                             StringComparison.InvariantCultureIgnoreCase)
                                       && ((query.GemType == GemType.Awakened && gemData.Name.StartsWith("Awakened"))
                                           || (query.GemType == GemType.Exceptional &&
                                               (gemData.Name.Contains("Enhance") || gemData.Name.Contains("Empower") ||
                                                gemData.Name.Contains("Enlighten")))
                                           || (query.GemType == GemType.Skill && !gemData.Name.EndsWith("Support"))
                                           || (query.GemType == GemType.RegularSupport &&
                                               gemData.Name.EndsWith("Support"))
                                           || query.GemType == GemType.All)
                     )
                     .Include(gemData => gemData.Gems)
                     .AsEnumerable()
                     .Where(gemData => gemData.RawCost() >= (query.PricePerTryFrom ?? decimal.MinValue)
                                       && gemData.RawCost() <= (query.PricePerTryTo ?? decimal.MaxValue)
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
                     .Skip(skipSize)
                     .Take(takeSize)
                     .ToList();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        //TODO maybe try to write this in SQL
        return result;
    }
}