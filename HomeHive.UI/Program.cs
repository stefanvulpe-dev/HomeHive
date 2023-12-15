using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HomeHive.UI;
using HomeHive.UI.Interfaces;
using HomeHive.UI.Services.Api;
using HomeHive.UI.Services.Authentication;
using HomeHive.UI.Utils;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEstateDataService, EstatesDataService>();

builder.Services.AddHttpClient("HomeHive.API", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["HomeHive.API:BaseAddress"] ?? "https://localhost:5001");
    })
    .ConfigurePrimaryHttpMessageHandler(services =>
    {
        var tokenService = services.GetRequiredService<ILocalStorageService>();
        return new CustomHttpClientHandler(tokenService, builder.Configuration["HomeHive.API:BaseAddress"]!);
    });

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();