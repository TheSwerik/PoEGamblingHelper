using Blazored.SessionStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PoEGamblingHelper3;
using PoEGamblingHelper3.Shared.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#if DEBUG
const string backendUrl = "https://localhost:7166/";
#endif
#if RELEASE
const string backendUrl = builder.HostEnvironment.BaseAddress + "api";
#endif

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(backendUrl) });
builder.Services.AddScoped<IGemService>(sp => new GemService(sp.GetService<HttpClient>()!));
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();