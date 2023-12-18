using HomeHive.Application.Features.Utilities.Commands.CreateUtility;

namespace HomeHive.Tests.ApplicationTests.Utilities.Commands.CreateCommand;

public class CreateUtilityDtoTests
{
    [Fact]
    public void CreateUtilityDto_SetPropertiesWithInitializer_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var createUtilityDto = new CreateUtilityDto();

        // Act
        var updatedUtilityName = "UpdatedUtilityName";

        createUtilityDto = createUtilityDto with
        {
            UtilityName = updatedUtilityName
        };

        // Assert
        Assert.Equal(createUtilityDto.Id, createUtilityDto.Id);
        Assert.Equal(updatedUtilityName, createUtilityDto.UtilityName);
    }
}