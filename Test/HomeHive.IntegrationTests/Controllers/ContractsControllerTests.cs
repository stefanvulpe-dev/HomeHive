using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using IntegrationTests.Base;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers;

public class ContractsControllerTests : BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Contracts";
    private Guid _estateId;
    private Guid _contractId;
    private Guid _contractIdToBeDeleted;

    public ContractsControllerTests():base("ContractsControllerTests")
    {
        InitIds().Wait();
    }

    private async Task InitIds()
    {
        InitializeAsync().Wait();
        var createContract = $"{BaseUrl}";
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        
        var estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "Image");
        var estatesResponse = await Client.PostAsJsonAsync("/api/v1/Estates", estateData);
        var estateResponseString = await estatesResponse.Content.ReadAsStringAsync();
        
        _estateId = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(estateResponseString)!.Estate.Id;
        
        var contractData = new ContractData(_estateId, "Rent", DateTime.Now, DateTime.Now, "Description");
        var contractResponse = await Client.PostAsJsonAsync(createContract, contractData);
        var contractResponseString = await contractResponse.Content.ReadAsStringAsync();
        _contractId = JsonConvert.DeserializeObject<CreateContractCommandResponse>(contractResponseString)!.Contract!
            .ContractId;
        
        estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "Image");
        estatesResponse = await Client.PostAsJsonAsync("/api/v1/Estates", estateData);
        estateResponseString = await estatesResponse.Content.ReadAsStringAsync();
        
        var estateId = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(estateResponseString)!.Estate.Id;
        
        contractData = new ContractData(estateId, "Rent", DateTime.Now, DateTime.Now, "Description");
        contractResponse = await Client.PostAsJsonAsync(createContract, contractData);
        contractResponseString = await contractResponse.Content.ReadAsStringAsync();
        _contractIdToBeDeleted = JsonConvert.DeserializeObject<CreateContractCommandResponse>(contractResponseString)!.Contract!
            .ContractId;
    }

    [Fact]
    public async Task GetAllContracts_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getAllContracts = $"{BaseUrl}/all";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        var response = await Client.GetAsync(getAllContracts);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetAllContractsQueryResponse>(responseString);
        // Assert
        result?.Contracts!.Count.Should().BeGreaterThan(4);
    }

    [Fact]
    public async Task GetContractsByUser_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getContractsByUser = $"{BaseUrl}/user";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        // Act
        var response = await Client.GetAsync(getContractsByUser);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GetAllContractsByUserIdResponse>();
        result?.Contracts!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateContract_WhenCalled_ReturnsCreatedResult()
    {
        // Arrange
        var createContract = $"{BaseUrl}";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        
         // Act
        var contractData = new ContractData(_estateId, "Rent", DateTime.Now, DateTime.Now, "Description");

        // Act
        var response = await Client.PostAsJsonAsync(createContract, contractData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateContract_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var updateContract = $"{BaseUrl}/{_contractId}";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        var contractData = new ContractData(Guid.Empty, "", null, null, "UpdatedDescription");

        // Act
        var response = await Client.PutAsJsonAsync(updateContract, contractData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteContract_WhenCalled_ReturnsNoContentResult()
    {
        // Arrange
        var deleteContract = $"{BaseUrl}/{_contractIdToBeDeleted}";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        // Act
        var response = await Client.DeleteAsync(deleteContract);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetContractById_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var getContractById = $"{BaseUrl}/{_contractId}";

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        // Act
        var response = await Client.GetAsync(getContractById);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeAnonymousType(responseString, new
        {
            contract = new
            {
                UserId = Guid.Empty,
                EstateId = Guid.Empty,
                ContractType = "",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Description = ""
            },
            isSuccess = false,
            message = "",
            validationsErrors = new Dictionary<string, string[]>()
        });
        result!.contract.Should().NotBeNull();
        result.contract.EstateId.Should().Be(_estateId);
    }
    
    [Fact]
public async Task CreateContract_WhenNotAuthorized_ReturnsUnauthorizedResult()
{
    // Arrange
    var createContract = $"{BaseUrl}";
    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

    // Act
    var contractData = new ContractData(_estateId, "Rent", DateTime.Now, DateTime.Now, "Description");
    var response = await Client.PostAsJsonAsync(createContract, contractData);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}

[Fact]
public async Task UpdateContract_WhenNotAuthorized_ReturnsUnauthorizedResult()
{
    // Arrange
    var updateContract = $"{BaseUrl}/{_contractId}";
    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");


    // Act
    var contractData = new ContractData(Guid.Empty, "", null, null, "UpdatedDescription");
    var response = await Client.PutAsJsonAsync(updateContract, contractData);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}

[Fact]
public async Task DeleteContract_WhenNotAuthorized_ReturnsUnauthorizedResult()
{
    // Arrange
    var deleteContract = $"{BaseUrl}/{_contractIdToBeDeleted}";
    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

    // Act
    var response = await Client.DeleteAsync(deleteContract);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}

[Fact]
public async Task GetContractById_WhenNotAuthorized_ReturnsUnauthorizedResult()
{
    // Arrange
    var getContractById = $"{BaseUrl}/{_contractId}";
    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");

    // Act
    var response = await Client.GetAsync(getContractById);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}

[Fact]
public async Task CreateContract_WhenBadRequest_ReturnsBadRequestResult()
{
    // Arrange
    var createContract = $"{BaseUrl}";

    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    // Act
    var contractData = new ContractData(Guid.Empty, "", null, null, "Description");
    var response = await Client.PostAsJsonAsync(createContract, contractData);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}

[Fact]
public async Task UpdateContract_WhenBadRequest_ReturnsBadRequestResult()
{
    // Arrange
    var updateContract = $"{BaseUrl}/{_contractId}";

    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    // Act
    var contractData = new ContractData(Guid.Empty, "", null, null, "");
    var response = await Client.PutAsJsonAsync(updateContract, contractData);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}

[Fact]
public async Task DeleteContract_WhenNotFound_ReturnsBadRequestResult()
{
    // Arrange
    var deleteContract = $"{BaseUrl}/{Guid.NewGuid()}";

    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    // Act
    var response = await Client.DeleteAsync(deleteContract);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}

[Fact]
public async Task GetContractById_WhenNotFound_ReturnsBadRequest()
{
    // Arrange
    var getContractById = $"{BaseUrl}/{Guid.NewGuid()}";

    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

    // Act
    var response = await Client.GetAsync(getContractById);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}
}

