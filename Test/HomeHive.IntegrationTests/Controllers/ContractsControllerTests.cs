using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Models;
using IntegrationTests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests.Controllers;

public class ContractsControllerTests: BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Contracts";
    private string? _token;

    private ContractsControllerTests()
    {
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

        _token = loginResult["value"]!["accessToken"]?.Value<string>()!;
    }
    
    [Fact]
    public async Task GetAllContracts_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getAllContracts = $"{BaseUrl}/all";
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var response = await Client.GetAsync(getAllContracts);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetAllContractsQueryResponse>(responseString);
        // Assert
        result?.Contracts!.Count.Should().Be(4);
    }
}