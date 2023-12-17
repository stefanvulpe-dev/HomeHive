using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class PhotoTests
{
    [Fact]
    public void CreatePhoto_WithValidData_ShouldCreatePhoto()
    {
        // Arrange
        var estate = Estate.Create(Guid.NewGuid(), new EstateData("House", "ForRent", "Test", "Test", 100, "Test", "Test", "Test", "Test")).Value;
        var objectName = "Test";

        // Act
        var result = Photo.Create(objectName, estate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.EstateId, estate.Id);
        Assert.Equal(result.Value.Estate, estate);
        Assert.Equal(result.Value.ObjectName, objectName);
    }
    
    [Fact]
    public void CreatePhoto_WithInvalidObjectName_ShouldNotCreatePhoto()
    {
        // Arrange
        var estate = Estate.Create(Guid.NewGuid(), new EstateData("House", "ForRent", "Test", "Test", 100, "Test", "Test", "Test", "Test")).Value;
        var objectName = "";

        // Act
        var result = Photo.Create(objectName, estate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Object name is not valid!", result.Error);
    }
    
    [Fact]
    public void CreatePhoto_WithInvalidEstate_ShouldNotCreatePhoto()
    {
        // Arrange
        var estate = Estate.Create(Guid.NewGuid(), new EstateData("House", "ForRent", "Test", "Test", 100, "Test", "Test", "Test", "Test")).Value;
        var objectName = "Test";

        // Act
        var result = Photo.Create(objectName, null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Estate is required.", result.Error);
    }
    
}