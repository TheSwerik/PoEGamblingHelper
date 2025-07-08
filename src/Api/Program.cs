using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.Cookie.Name = "AnalyticsCookie";
           options.Cookie.HttpOnly = true;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
           options.ExpireTimeSpan = TimeSpan.FromDays(30);
           options.SlidingExpiration = true;
           options.LoginPath = "/analytics/login";

           options.Events.OnRedirectToLogin = ctx =>
           {
               ctx.Response.StatusCode = 401;
               return Task.CompletedTask;
           };

           options.Events.OnRedirectToAccessDenied = ctx =>
           {
               ctx.Response.StatusCode = 403;
               return Task.CompletedTask;
           };
       });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsBuilder => corsBuilder.WithOrigins(app.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials());

app.UseRateLimiter();
app.UseOutputCache();
app.MapControllers();
app.MigrateDatabase();
app.UseAnalytics();
app.UseAuthentication();
app.UseAuthorization();

app.Run();