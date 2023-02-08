global using Backend.Data;
using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;
using Backend.Exceptions;
using Backend.Service;
using Microsoft.EntityFrameworkCore;
using Shared.Entity;

#if DEBUG
Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
#endif

#region Builder

var builder = WebApplication.CreateBuilder(args);

#region Web

builder.Services
       .AddRateLimiter(limiterOptions =>
                       {
                           limiterOptions.OnRejected =
                               (context, _) =>
                               {
                                   if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                                       context.HttpContext.Response.Headers.RetryAfter =
                                           ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                                   context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                                   context.HttpContext.RequestServices
                                          .GetService<ILoggerFactory>()?
                                          .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                                          .LogWarning("OnRejected: {GetUserEndPoint}",
                                                      GetUserEndPoint(context.HttpContext));
                                   return new ValueTask();
                               };

                           limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(
                               context =>
                               {
                                   var remoteIpAddress = context.Connection.RemoteIpAddress;
                                   if (IPAddress.IsLoopback(remoteIpAddress!))
                                       return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);

                                   return RateLimitPartition.GetTokenBucketLimiter(remoteIpAddress!,
                                       _ => new TokenBucketRateLimiterOptions
                                            {
                                                TokenLimit = int.Parse(builder.Configuration["RateLimit:TokenLimit"]!),
                                                ReplenishmentPeriod =
                                                    TimeSpan.FromSeconds(
                                                        int.Parse(builder.Configuration[
                                                                      "RateLimit:ReplenishmentPeriodSeconds"]!)),
                                                TokensPerPeriod =
                                                    int.Parse(builder.Configuration["RateLimit:TokensPerPeriod"]!),
                                                AutoReplenishment = true
                                            });
                               });
                       });
builder.Services.AddControllers(options => { options.Filters.Add<HttpResponseExceptionFilter>(); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

#region Data

#if DEBUG
builder.Services.AddDbContext<ApplicationDbContext>(opt => { opt.UseInMemoryDatabase("PoEGamblingHelper"); });
#elif RELEASE
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection") + $"Password={builder.Configuration["POSTGRES_PASSWORD"]};")
);
#endif
builder.Services.AddScoped<IGemDataRepository, GemDataRepository>();
builder.Services.AddScoped<IRepository<GemTradeData, long>, Repository<GemTradeData, long>>();
builder.Services.AddScoped<IRepository<Currency, string>, Repository<Currency, string>>();
builder.Services.AddScoped<IRepository<League, Guid>, Repository<League, Guid>>();
builder.Services.AddScoped<IRepository<TempleCost, Guid>, Repository<TempleCost, Guid>>();

#endregion

#region Service

builder.Services.AddSingleton<IPoeDataFetchService, PoeDataFetchService>();
builder.Services.AddSingleton<IPoeDataService, PoeDataService>();

builder.Services.AddHostedService<InitService>();

builder.Services.AddOutputCache(options =>
                                {
                                    options.AddBasePolicy(cacheBuilder => cacheBuilder
                                                                          .Expire(TimeSpan.FromMinutes(
                                                                              PoeDataFetchService
                                                                                  .PoeNinjaFetchMinutes))
                                                                          .Tag("FetchData"));
                                });

#endregion

#endregion


#region App

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(corsBuilder =>
                corsBuilder.WithOrigins(app.Configuration["AllowedOrigins"]!).AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();

app.UseRateLimiter();
app.UseOutputCache();
app.MapControllers();

#region Migration

#if RELEASE
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}
#endif

#endregion

app.Run();

#endregion

#region Helper Methods

static string GetUserEndPoint(HttpContext context)
{
    return $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
           + $" {context.Connection.RemoteIpAddress}";
}

static string GetTicks() { return (DateTime.Now.Ticks & 0x11111).ToString("00000"); }

#endregion