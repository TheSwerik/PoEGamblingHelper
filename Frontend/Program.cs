using System.Globalization;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PoEGamblingHelper3;
using PoEGamblingHelper3.Service;

Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient
                                {
                                    BaseAddress = new Uri(builder.Configuration["BackendUrl"] ??
                                                          builder.HostEnvironment.BaseAddress + "/api/")
                                });
builder.Services.AddScoped<IGemService, GemService>();
builder.Services.AddScoped<ITempleCostService, TempleCostService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IScrollInfoService, ScrollInfoService>();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();