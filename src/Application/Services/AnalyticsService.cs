﻿using System.Security.Cryptography;
using System.Text;
using Domain.Entity.Analytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IApplicationDbContextFactory _applicationDbContextFactory;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(IApplicationDbContextFactory applicationDbContextFactory, ILogger<AnalyticsService> logger)
    {
        _applicationDbContextFactory = applicationDbContextFactory;
        _logger = logger;
    }

    public async Task AddView(string? ipAddress)
    {
        if (ipAddress is null) return;
        var ipHash = SHA512.HashData(Encoding.UTF8.GetBytes(ipAddress));
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var view = new View { IpHash = ipHash, TimeStamp = today };
        using var ctx = _applicationDbContextFactory.CreateDbContext();
        if (await ctx.View.AsNoTracking().AnyAsync(v => v.IpHash.Equals(ipHash) && v.TimeStamp == today)) return;

        await ctx.View.AddAsync(view);
        await ctx.SaveChangesAsync();
    }

    public async Task LogYesterdaysViews()
    {
        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        using var ctx = _applicationDbContextFactory.CreateDbContext();
        var viewCount = await ctx.View.Where(v => v.TimeStamp == yesterday).CountAsync();
        _logger.LogInformation("{Views} People used the website on the {YesterdayDay}.{YesterdayMonth}.{YesterdayYear}",
                               viewCount, yesterday.Day, yesterday.Month, yesterday.Year);
        var views = await ctx.View.Where(v => v.TimeStamp <= yesterday).ToArrayAsync();
        ctx.View.RemoveRange(views);
        await ctx.SaveChangesAsync();
    }
}