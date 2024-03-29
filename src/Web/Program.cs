using System.Globalization;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;
using Web.Services.Implementations;
using Web.Services.Interfaces;

Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(_ => new HttpClient
                                {
                                    BaseAddress = new Uri(builder.Configuration.GetValue<string>("BackendUrl") ??
                                                          $"{builder.HostEnvironment.BaseAddress}/api/")
                                });
builder.Services.AddScoped<IGemService, GemService>();
builder.Services.AddScoped<ITempleCostService, TempleCostService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IScrollInfoService, ScrollInfoService>();

await builder.Build().RunAsync();