using Blazored.LocalStorage;
using HomeHive.UI;
using HomeHive.UI.Interfaces;
using HomeHive.UI.Services.Api;
using HomeHive.UI.Services.Authentication;
using HomeHive.UI.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<HomeHiveAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    s => s.GetRequiredService<HomeHiveAuthStateProvider>());
builder.Services.AddScoped<IEstateDataService, EstatesDataService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IContractDataService, ContractDataService>();
builder.Services.AddHttpClient("HomeHive.API",
        client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["HomeHive.API:BaseAddress"] ?? "https://localhost:5001");
        })
    .ConfigurePrimaryHttpMessageHandler(services =>
    {
        var tokenService = services.GetRequiredService<ILocalStorageService>();
        return new CustomHttpClientHandler(tokenService);
    });

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();