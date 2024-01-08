using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeHive.Application.Features.Rooms.Commands.CreateRoom;
using HomeHive.Application.Features.Rooms.Queries.GetAllRooms;
using HomeHive.Application.Features.Rooms.Queries.GetRoomById;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using IntegrationTests.Base;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers;

public class RoomsControllerTests: BaseApplicationContextTexts
{
    private const string BaseUrl = "/api/v1/Rooms";
    
    [Fact]
    public async Task CreateRoom_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        const string roomName = "Balcony";
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, roomName);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateRoomCommandResponse>(responseString);
        // Assert
        Assert.NotNull(result?.Room);
    }
    
    [Fact]
    public async Task CreateRoom_WhenCalled_ReturnsBadRequestResult()
    {
        // Arrange
        const string roomName = "Bedroom";
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, roomName);
        // Act
        //Assert
        Assert.Equal(400, (int)response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllRooms_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.GetAsync(BaseUrl);
        // Act
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<GetAllRoomsResponse>(responseString);
        // Assert
        Assert.NotNull(result?.Rooms);
        Assert.NotEmpty(result.Rooms);
    }
    
    [Fact]
    public async Task GetByIdRoom_WhenCalled_ReturnsOkResult()
    {
        // Arrange 
        const string roomName = "Balcony";
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await Client.PostAsJsonAsync(BaseUrl, roomName);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateRoomCommandResponse>(responseString);
        var roomId = result?.Room.Id;
        
        response = await Client.GetAsync($"{BaseUrl}/{roomId}");
        // Act
        response.EnsureSuccessStatusCode();
        responseString = await response.Content.ReadAsStringAsync();
        var getByIdResult = JsonConvert.DeserializeObject<GetRoomByIdResponse>(responseString);
        
        // Assert
        Assert.NotNull(getByIdResult?.Room);
        Assert.Equal(roomId, getByIdResult.Room.Id);
        Assert.Equal(roomName, getByIdResult.Room.RoomType);
        
    }
}