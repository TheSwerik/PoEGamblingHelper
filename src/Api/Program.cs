using System.Globalization;
using Api.Filters;

#if DEBUG
Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add<HttpResponseExceptionFilter>(); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//TODO call Infrastructure init
builder.Services.AddOutputCache(options =>
                                {
                                    options.AddBasePolicy(
                                        cacheBuilder => cacheBuilder
                                                        .Expire(TimeSpan.FromMinutes(
                                                                    int.Parse(builder.Configuration["FetchMinutes"]!)))
                                                        .Tag("FetchData"));
                                });

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
app.UseAuthorization();

app.UseRateLimiter();
app.UseOutputCache();
app.MapControllers();

//TODO run Infrastructure init runtime (migrations)

app.Run();