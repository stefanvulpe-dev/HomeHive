using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Models;
using IntegrationTests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace IntegrationTests.Controllers;

public class ContractsControllerTests: BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Contracts";
    
    
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