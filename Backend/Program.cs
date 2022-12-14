global using Backend.Data;
using Backend.Model;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

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
// opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection"))
// );
builder.Services.AddScoped<IRepository<GemData>, Repository<GemData>>();
builder.Services.AddScoped<IRepository<League>, Repository<League>>();

#endregion

#region Service

builder.Services.AddSingleton<IPoeDataFetchService, PoeDataFetchService>();
builder.Services.AddSingleton<IPoeDataService, PoeDataService>();

builder.Services.AddHostedService<InitService>();

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

app.MapControllers();
app.Run();

#endregion