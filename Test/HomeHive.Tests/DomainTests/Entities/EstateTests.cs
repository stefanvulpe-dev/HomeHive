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
            EstateType: "House",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ownerId, result.Value.OwnerId);
        Assert.Equal(EstateType.House, result.Value.EstateType);
        Assert.Equal(EstateCategory.ForRent, result.Value.EstateCategory);
        Assert.Equal(estateData.Name, result.Value.Name);
        Assert.Equal(estateData.Location, result.Value.Location);
        Assert.Equal(estateData.Price, result.Value.Price);
        Assert.Equal(estateData.TotalArea, result.Value.TotalArea);
        Assert.Equal(estateData.Utilities, result.Value.Utilities);
        Assert.Equal(estateData.Description, result.Value.Description);
        Assert.Equal(estateData.Image, result.Value.Image);
        
        Assert.Null(result.Value.Photos);
        Assert.Null(result.Value.Rooms);
        Assert.Null(result.Value.Contracts);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidOwnerId_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.Empty;
        var estateData = new EstateData(
            EstateType: "Apartment",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("OwnerId is required.", result.Error);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidEstateType_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "NotExistentEstateType",
            EstateCategory: "ForSale",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("EstateType is not valid.", result.Error);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidEstateCategory_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Villa",
            EstateCategory: "NotExistentEstateCategory",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("EstateCategory is not valid.", result.Error);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidName_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Cottage",
            EstateCategory: "ForRent",
            Name: "",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Name is required.", result.Error);
    }
    
    [Fact]
    public void CreateEstate_WithInvalidLocation_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Farmhouse",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Location is required.", result.Error);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidPrice_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Bungalow",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: -2,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Price is required.", result.Error);
    }
    
    [Fact]
    public void CreateEstate_WithInvalidTotalArea_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Townhouse",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Total Area is required.", result.Error);
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidUtilities_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Penthouse",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "",
            Description: "Test Description",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Utilities is required.", result.Error);
        
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidDescription_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Studio",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "",
            Image: "Test Image");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Description is required.", result.Error);
        
        
    }
    
    [Fact]
    public void CreateEstate_WithInvalidImage_ShouldFail()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var estateData = new EstateData(
            EstateType: "Duplex",
            EstateCategory: "ForRent",
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "");

        // Act
        var result = Estate.Create(ownerId, estateData);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Image is required.", result.Error);
        
        
    }
    
    [Fact]
    public void UpdateEstate_WithValidData_ShouldSucceed()
    {
        // Arrange
        var estate = Estate.Create(Guid.NewGuid(), new EstateData(
            EstateType: EstateType.Bungalow.ToString(),
            EstateCategory: EstateCategory.ForSale.ToString(),
            Name: "Test Estate",
            Location: "Test Location",
            Price: 1000,
            TotalArea: "100m2",
            Utilities: "Test Utilities",
            Description: "Test Description",
            Image: "Test Image"
            )).Value;
        
        var estateData = new EstateData(
            EstateType: "Apartment",
            EstateCategory: "ForSale",
            Name: "Updated Test Estate",
            Location: "Updated Test Location",
            Price: 2000,
            TotalArea: "200m2",
            Utilities: "Updated Test Utilities",
            Description: "Updated Test Description",
            Image: "Updated Test Image");

        // Act
        estate.UpdateEstate(estateData);

        // Assert
        Assert.Equal(EstateType.Apartment, estate.EstateType);
        Assert.Equal(EstateCategory.ForSale, estate.EstateCategory);
        Assert.Equal(estateData.Name, estate.Name);
        Assert.Equal(estateData.Location, estate.Location);
        Assert.Equal(estateData.Price, estate.Price);
        Assert.Equal(estateData.TotalArea, estate.TotalArea);
        Assert.Equal(estateData.Utilities, estate.Utilities);
        Assert.Equal(estateData.Description, estate.Description);
        Assert.Equal(estateData.Image, estate.Image);
        
    }
}