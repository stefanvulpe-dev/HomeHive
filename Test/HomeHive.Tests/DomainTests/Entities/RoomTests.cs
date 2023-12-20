using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class RoomTests
{
    [Fact]
    public void CreateRoom_WithValidData_ShouldCreateRoom()
    {
        // Act && Arrange
        var result = Room.Create(RoomType.LivingRoom.ToString());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(RoomType.LivingRoom, result.Value.RoomType);
    }

    [Fact]
    public void CreateRoom_WithInvalidRoomType_ShouldNotCreateRoom()
    {
        // Act && Arrange
        var result = Room.Create("");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("RoomType is not valid.", result.Message);
    }
}