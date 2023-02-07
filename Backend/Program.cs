global using Backend.Data;
using System.Globalization;
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