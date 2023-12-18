using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.DomainTests.EntitiesUtils;

public class RoomDataTests
{
    private readonly IEstateRepository _estateRepository = Substitute.For<IEstateRepository>();

    [Fact]
    public void RoomData_SetProperties_ShouldModifyProperties()
    {
        // Arrange
        var roomData = new RoomData(null, null, 0, 0, null);

        // Act
        roomData = roomData with { Name = "Updated Room Name", Capacity = 10 };

        // Assert
        Assert.Equal("Updated Room Name", roomData.Name);
        Assert.Equal(10, roomData.Capacity);
    }

    [Fact]
    public void RoomData_WithValidValues_ShouldCreateInstance()
    {
        // Arrange
        var estateData = new EstateData(
            EstateType.Apartment.ToString(),
            EstateCategory.ForSale.ToString(),
            "Test Estate",
            "Test Location",
            100,
            "160m2",
            ["Test Utilities"],
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estateResult = Estate.Create(Guid.NewGuid(), utilities, estateData);

        _estateRepository.FindByIdAsync(Arg.Any<Guid>())
            .Returns(Result<Estate>.Success(estateResult.Value));

        var name = "Living Room";
        var roomType = "Living";
        var capacity = 4;
        var size = 25;
        var estate = estateResult.Value;

        // Act
        var roomData = new RoomData(name, roomType, capacity, size, estate);

        // Assert
        Assert.Equal(name, roomData.Name);
        Assert.Equal(roomType, roomData.RoomType);
        Assert.Equal(capacity, roomData.Capacity);
        Assert.Equal(size, roomData.Size);
        Assert.Equal(estate, roomData.Estate);
    }

    [Fact]
    public void RoomData_WithNullValues_ShouldCreateInstanceWithNulls()
    {
        // Act
        var roomData = new RoomData(null, null, 0, 0, null);

        // Assert
        Assert.Null(roomData.Name);
        Assert.Null(roomData.RoomType);
        Assert.Equal(0, roomData.Capacity);
        Assert.Equal(0, roomData.Size);
        Assert.Null(roomData.Estate);
    }
}