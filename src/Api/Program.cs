using System.Globalization;
using PoEGamblingHelper.Api.Configuration;
using PoEGamblingHelper.Api.Filters;
using PoEGamblingHelper.Api.Middleware;
using PoEGamblingHelper.Infrastructure;
using PoEGamblingHelper.Infrastructure.Database;

#if DEBUG
Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddConfiguredRateLimiter(builder.Configuration);
builder.Services.AddControllers(options => { options.Filters.Add<HttpExceptionResponseFilter>(); });
builder.Services.AddCache(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsBuilder => corsBuilder.WithOrigins(app.Configuration["AllowedOrigins"]!)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());

app.UseRateLimiter();
app.UseOutputCache();
app.MapControllers();
app.MigrateDatabase();
app.UseAnalytics();

app.Run();