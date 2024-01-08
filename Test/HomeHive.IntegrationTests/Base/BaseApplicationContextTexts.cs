using System.Net.Http.Json;
using HomeHive.Application.Models;
using HomeHive.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;

namespace IntegrationTests.Base;

public abstract class BaseApplicationContextTexts : IAsyncDisposable
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;
    protected  string? Token;

    protected BaseApplicationContextTexts(string databaseSuffix = "")
    {
        Application = new WebApplicationFactory<Program>();
        Application = Application.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<HomeHiveContext>));
                services.AddDbContext<HomeHiveContext>(options =>
                {
                    options.UseInMemoryDatabase($"HomeHiveDbForTesting_{databaseSuffix}");
                });

                services.Configure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                        options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    }
                );
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<HomeHiveContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    Seed.InitializeDbForTests(db);
                }
            });
        });
        Client = Application.CreateClient();
        InitializeAsync().Wait();
    }

    private async Task InitializeAsync()
    {
            var authenticateEndpoint = "/api/v1/Authentication/login";

            var fakeUser = new LoginModel
            {
                //Provide valid credentials of an existing user
                UserName = "octav123",
                Password = "!Scr00l33"
            };

            var loginResponse = await Client
                .PostAsJsonAsync(authenticateEndpoint, fakeUser);
            
            loginResponse.EnsureSuccessStatusCode();
            var stringResponse = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JObject.Parse(stringResponse);

            Token = loginResult["value"]!["accessToken"]?.Value<string>()!;
        
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Application.DisposeAsync();
    }
}