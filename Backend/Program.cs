global using Backend.Data;
using Backend.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(opt => { opt.UseInMemoryDatabase("PoEGamblingHelper"); });
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddHostedService<ScheduledService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();