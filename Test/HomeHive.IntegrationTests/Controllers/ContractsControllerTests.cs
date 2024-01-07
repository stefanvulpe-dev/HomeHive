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
    
    [Fact]
    public async Task GetAllContracts_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getAllContracts = $"{BaseUrl}/all";
        
        var authenticateEndpoint = "/api/v1/Authentication/login";

        var fakeUser = new LoginModel
        {
            UserName = "octav123",
            Password = "!Scr00l33"
        };

        var loginResponse = await Client
            .PostAsJsonAsync(authenticateEndpoint, fakeUser);

        // Assert
        loginResponse.EnsureSuccessStatusCode();
        var stringResponse = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JObject.Parse(stringResponse);

        var token = loginResult["value"]!["accessToken"]?.Value<string>();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.GetAsync(getAllContracts);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetAllContractsQueryResponse>(responseString);
        // Assert
        result?.Contracts.Should().NotBeNullOrEmpty();

    }
    
    
}