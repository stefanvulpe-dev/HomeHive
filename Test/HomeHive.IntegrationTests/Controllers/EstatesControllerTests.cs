using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using IntegrationTests.Base;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace IntegrationTests.Controllers;

public class EstatesControllerTests: BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Estates";
    
    [Fact]
    public async Task CreateEstate_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "asodjaosjfoajso");
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, estateData);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(responseString);
        // Assert
        Assert.NotNull(result?.Estate);
    }
    
    [Fact]
    public async Task UpdateEstate_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "asodjaosjfoajso");
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, estateData);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(responseString);

        var updateByIdEstate = $"{BaseUrl}/{result?.Estate.Id}";
        
        var updateEstateData = new EstateData("Apartment", "ForSale", "Name2", "Location2", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "asodjaosjfoajso");
        
        //Act
        var updateResponse = await Client.PutAsJsonAsync(updateByIdEstate, updateEstateData);
        updateResponse.EnsureSuccessStatusCode();
        var updateResponseString = await updateResponse.Content.ReadAsStringAsync();
        var updateResult = JsonConvert.DeserializeObject<UpdateEstateCommandResponse>(updateResponseString);
        //Assert
        Assert.NotNull(updateResult?.Estate);
        Assert.Equal(updateEstateData.EstateType, updateResult?.Estate.EstateType);
        Assert.Equal(updateEstateData.EstateCategory, updateResult?.Estate.EstateCategory);
        Assert.Equal(updateEstateData.Name, updateResult?.Estate.Name);
        Assert.Equal(updateEstateData.Location, updateResult?.Estate.Location);
        
    }
    
    [Fact]
    public async Task GetByIdEstate_WhenCalled_ReturnsOkResult()
    {
        
        // Arrange
        var estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "asodjaosjfoajso");
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, estateData);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(responseString);

        var getByIdEstate = $"{BaseUrl}/{result?.Estate.Id}";
        
        
        var getResponse = await Client.GetAsync(getByIdEstate);
        // Act
        response.EnsureSuccessStatusCode();
        var getResponseString = await getResponse.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<GetEstateByIdResponse>(getResponseString);
        // Assert
        Assert.NotNull(getResult?.Estate);
    }
    
    
    
    [Fact]
    public async Task DeleteEstate_WhenCalled_ReturnsEmpty()
    {
        //Arrange
        var estateData = new EstateData("House", "ForRent", "Name1", "Location1", 20, "40m2", new List<string>{"Water"}, new Dictionary<string, int>{{"Kitchen", 2}}, "Description1", "asodjaosjfoajso");
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, estateData);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateEstateCommandResponse>(responseString);

        var deleteByIdEstate = $"{BaseUrl}/{result?.Estate.Id}";
        //Act
        var deleteResponse = await Client.DeleteAsync(deleteByIdEstate);
        
        //Assert
        Assert.Equal(204, (int)deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task GetAllEstates_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.GetAsync(BaseUrl);
        // Act
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetAllEstates_WhenCalled_ReturnsNonEmptyList()
    {
        // Arrange
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.GetAsync(BaseUrl);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetAllEstatesResponse>(responseString);
        // Assert
        Assert.NotNull(result?.Estates);
        Assert.NotEmpty(result.Estates);
    }


}