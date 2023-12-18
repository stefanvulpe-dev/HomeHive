using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class RoomTests
{
    [Fact]
    public void CreateRoom_WithValidData_ShouldCreateRoom()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData("House", "ForRent",
                "Test", "Test", 100, "Test",
                ["Test"], "Test", "Test")).Value;
        var roomData = new RoomData("Test", RoomType.LivingRoom.ToString(), 200, 200, estate);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.EstateId, estate.Id);
        Assert.Equal(result.Value.Estate, estate);
        Assert.Equal(result.Value.Name, roomData.Name);
        Assert.Equal(result.Value.RoomType, Enum.Parse<RoomType>(roomData.RoomType!));
        Assert.Equal(result.Value.Capacity, roomData.Capacity);
        Assert.Equal(result.Value.Size, roomData.Size);
    }

    [Fact]
    public void CreateRoom_WithInvalidName_ShouldNotCreateRoom()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData("House", "ForRent",
                "Test", "Test", 100, "Test",
                ["Test"], "Test", "Test")).Value;
        var roomData = new RoomData("", RoomType.LivingRoom.ToString(), 200, 200, estate);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Name is not valid.", result.Message);
    }

    [Fact]
    public void CreateRoom_WithInvalidRoomType_ShouldNotCreateRoom()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData("House", "ForRent",
                "Test", "Test", 100, "Test",
                ["Test"], "Test", "Test")).Value;
        var roomData = new RoomData("Test", "", 200, 200, estate);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("RoomType is not valid.", result.Message);
    }

    [Fact]
    public void CreateRoom_WithInvalidCapacity_ShouldNotCreateRoom()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData("House", "ForRent",
                "Test", "Test", 100, "Test",
                ["Test"], "Test", "Test")).Value;
        var roomData = new RoomData("Test", RoomType.LivingRoom.ToString(), -20, 200, estate);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Capacity should be greater than 0.", result.Message);
    }

    [Fact]
    public void CreateRoom_WithInvalidSize_ShouldNotCreateRoom()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData("House", "ForRent",
                "Test", "Test", 100, "Test",
                ["Test"], "Test", "Test")).Value;
        var roomData = new RoomData("Test", RoomType.LivingRoom.ToString(), 200, -20, estate);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Size should be greater than 0.", result.Message);
    }

    [Fact]
    public void CreateRoom_WithInvalidEstate_ShouldNotCreateRoom()
    {
        // Arrange
        var roomData = new RoomData("Test", RoomType.LivingRoom.ToString(), 200, 200, null);

        // Act
        var result = Room.Create(roomData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Estate is required.", result.Message);
    }
}