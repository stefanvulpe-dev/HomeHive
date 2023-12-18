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
        var roomData = new RoomData
        {
            EstateId = Guid.Empty,
            RoomType = "Living",
            Quantity = 4
        };

        // Act
        roomData = roomData with { RoomType = "Updated Room Type" };

        // Assert
        Assert.Equal("Updated Room Type", roomData.RoomType);
    }
    
    [Fact]
    public void RoomData_WithValidValues_ShouldCreateInstance()
    {
        // Arrange
        EstateData estateData = new EstateData(
            EstateType.Apartment.ToString(),
            EstateCategory.ForSale.ToString(),
            "Test Estate",
            "Test Location",
            100,
            "160m2",
            ["Test Utilities"],
            "Test Description",
            "Test Image");
        
        Result<Utility> utilityResult = Utility.Create("Test Utilities");
        List<Utility> utilities = new List<Utility> { utilityResult.Value };
        
        Result<Estate> estateResult = Estate.Create(Guid.NewGuid(), utilities, estateData);
        
        _estateRepository.FindByIdAsync(Arg.Any<Guid>())
            .Returns(Result<Estate>.Success(estateResult.Value));

        // Act
        var roomData = new RoomData
        {
            EstateId = estateResult.Value.Id,
            RoomType = "Living",
            Quantity = 4
        };
       
        // Assert
        Assert.Equal(estateResult.Value.Id, roomData.EstateId);
        Assert.Equal("Living", roomData.RoomType);
        Assert.Equal(4, roomData.Quantity);
    }

    [Fact]
    public void RoomData_WithNullValues_ShouldCreateInstanceWithNulls()
    {
        // Act
        var roomData = new RoomData
        {
            EstateId = Guid.Empty,
            RoomType = null,
            Quantity = 0
        };

        // Assert
        Assert.Equal(Guid.Empty, roomData.EstateId);
        Assert.Null(roomData.RoomType);
        Assert.Equal(0, roomData.Quantity);
    }
}