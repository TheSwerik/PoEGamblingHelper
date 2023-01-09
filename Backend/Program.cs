global using Backend.Data;
using System.Globalization;
using Backend.Service;
using Microsoft.EntityFrameworkCore;
using Model;

#if DEBUG
Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
#endif

#region Builder

var builder = WebApplication.CreateBuilder(args);

#region Web

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

#region Data

builder.Services.AddDbContext<ApplicationDbContext>(opt => { opt.UseInMemoryDatabase("PoEGamblingHelper"); });
// builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(
// opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection"))
// );
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
app.Run();

#endregion