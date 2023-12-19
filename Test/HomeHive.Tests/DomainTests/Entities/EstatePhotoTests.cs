using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class EstatePhotoTests
{
    [Fact]
    public void CreatePhoto_WithValidData_ShouldCreatePhoto()
    {
        // Arrange
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estateData = new EstateData("House", "ForRent",
            "Test", "Test", 100, "Test",
            new List<string> { "Test" }, "Test", "Test");

        var estate = Estate.Create(Guid.NewGuid(), utilities, estateData).Value;
        var objectName = "Test";

        // Act
        var result = EstatePhoto.Create(objectName, estate);

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
        var utilityResult = Utility.Create("Test Utilities");
        var utilities = new List<Utility> { utilityResult.Value };

        var estateData = new EstateData("House", "ForRent",
            "Test", "Test", 100, "Test",
            new List<string> { "Test" }, "Test", "Test");

        var estate = Estate.Create(Guid.NewGuid(), utilities, estateData).Value;
        var objectName = "";

        // Act
        var result = EstatePhoto.Create(objectName, estate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Object name is not valid!", result.Message);
        Assert.Contains("Object name cannot be empty", result.ValidationErrors["ObjectName"]);
    }


    [Fact]
    public void CreatePhoto_WithInvalidEstate_ShouldNotCreatePhoto()
    {
        // Arrange
        var objectName = "Test";

        // Act
        var result = EstatePhoto.Create(objectName, null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Estate is required.", result.Message);
    }
}