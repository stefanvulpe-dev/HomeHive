using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class EstateTests
{
    [Fact]
    public void CreateEstate_WithValidData_ShouldSucceed()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "House",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ownerId, result.Value.OwnerId);
        Assert.Equal(EstateType.House, result.Value.EstateType);
        Assert.Equal(EstateCategory.ForRent, result.Value.EstateCategory);
        Assert.Equal(estateData.Name, result.Value.Name);
        Assert.Equal(estateData.Location, result.Value.Location);
        Assert.Equal(estateData.Price, result.Value.Price);
        Assert.Equal(estateData.TotalArea, result.Value.TotalArea);
        Assert.Equal(utilities, result.Value.Utilities);
        Assert.Equal(estateData.Description, result.Value.Description);
        Assert.Equal(estateData.EstateAvatar, result.Value.EstateAvatar);

        Assert.Null(result.Value.EstatePhotos);
        Assert.Null(result.Value.Rooms);
        Assert.Null(result.Value.Contracts);
    }

    [Fact]
    public void CreateEstate_WithInvalidOwnerId_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.Empty;
        var estateData = new EstateData(
            "Apartment",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("OwnerId is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidEstateType_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "NotExistentEstateType",
            "ForSale",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("EstateType is not valid.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidEstateCategory_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Villa",
            "NotExistentEstateCategory",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("EstateCategory is not valid.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidName_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Cottage",
            "ForRent",
            "",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Name is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidLocation_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Farmhouse",
            "ForRent",
            "Test Estate",
            "",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Location is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidPrice_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Bungalow",
            "ForRent",
            "Test Estate",
            "Test Location",
            -2,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Price is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidTotalArea_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Townhouse",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Total Area is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidUtilitiesNames_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Penthouse",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{""},
            "Test Description",
            "Test Image");

        var utilityResult = Utility.Create("");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Utility name is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidUtilitiesList_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Penthouse",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            null,
            "Test Description",
            "Test Image");

        List<Utility> utilities = null!;

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Utilities are required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidDescription_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Studio",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "",
            "Test Image");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Description is required.", result.Message);
    }

    [Fact]
    public void CreateEstate_WithInvalidImage_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            "Duplex",
            "ForRent",
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "");

        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        // Act
        var result = Estate.Create(ownerId, utilities, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Estate Avatar is required.", result.Message);
    }

    [Fact]
    public void UpdateEstate_WithValidData_ShouldSucceed()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estate = Estate.Create(Guid.NewGuid(), utilities, new EstateData(
            EstateType.Bungalow.ToString(),
            EstateCategory.ForSale.ToString(),
            "Test Estate",
            "Test Location",
            1000,
            "100m2",
            new List<String>{"Test Utilities"},
            "Test Description",
            "Test Image"
        )).Value;

        var utilityResult1 = Utility.Create("Updated Test Utilities");
        utilities = new List<Utility> { utilityResult1.Value };

        var estateData = new EstateData(
            "Apartment",
            "ForSale",
            "Updated Test Estate",
            "Updated Test Location",
            2000,
            "200m2",
            new List<String>{"Updated Test Utilities"},
            "Updated Test Description",
            "Updated Test Image");

        // Act
        estate.Update(utilities, estateData);

        // Assert
        Assert.Equal(EstateType.Apartment, estate.EstateType);
        Assert.Equal(EstateCategory.ForSale, estate.EstateCategory);
        Assert.Equal(estateData.Name, estate.Name);
        Assert.Equal(estateData.Location, estate.Location);
        Assert.Equal(estateData.Price, estate.Price);
        Assert.Equal(estateData.TotalArea, estate.TotalArea);
        Assert.Equal(utilities, estate.Utilities);
        Assert.Equal(estateData.Description, estate.Description);
        Assert.Equal(estateData.EstateAvatar, estate.EstateAvatar);
    }
}