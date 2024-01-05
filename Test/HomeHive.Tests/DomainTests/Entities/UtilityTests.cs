using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class UtilityTests
{
    [Fact]
    public void CreateUtility_WithValidName_ShouldCreateUtilityInstance()
    {
        // Arrange
        var utilityName = "Water";

        // Act
        var result = Utility.Create(utilityName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(utilityName, result.Value.UtilityType.ToString());
        Assert.Null(result.Value.Estates!);
    }

    [Fact]
    public void CreateUtility_WithEmptyName_ShouldReturnFailureResult()
    {
        // Arrange
        var utilityName = "";

        // Act
        var result = Utility.Create(utilityName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("UtilityType is not valid.", result.Message);
    }

    [Fact]
    public void UpdateUtility_WithValidName_ShouldUpdateUtilityName()
    {
        // Arrange
        var utility = Utility.Create("Water").Value;
        var newUtilityName = "Electricity";

        // Act
        utility.Update(newUtilityName);

        // Assert
        Assert.Equal(newUtilityName, utility.UtilityType.ToString());
    }

    [Fact]
    public void AddEstateToUtility_ShouldAddEstateToList()
    {
        // Arrange
        var utility = Utility.Create("Water").Value;
        var utilities = new List<Utility> { utility };
        var estate = Estate.Create(Guid.NewGuid(), utilities,
            new EstateData(
                EstateType.Apartment.ToString(),
                EstateCategory.ForRent.ToString(),
                "Test",
                "Test",
                100,
                "160m2",
                ["test", "Water"],
                new Dictionary<string, int>(){ {"Test", 1 }, {"Test1", 2} },
                "Test",
                "Test")).Value;

        // Act
        utility.Estates = new List<Estate> { estate };

        // Assert
        Assert.Single(utility.Estates);
        Assert.Contains(estate, utility.Estates);
    }
}