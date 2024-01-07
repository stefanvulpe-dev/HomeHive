using HomeHive.Application.Features.Contracts.Queries;
using IntegrationTests.Base;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers;

public class ContractsControllerTests: BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Contracts";
    
    [Fact]
    public async Task GetAllContracts_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getAllContracts = $"{BaseUrl}/all";
        var response = await Client.GetAsync(getAllContracts);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ContractDto>>(responseString);
        // Assert
        result?.Count().Should().Be(4);

    }
    
    
}