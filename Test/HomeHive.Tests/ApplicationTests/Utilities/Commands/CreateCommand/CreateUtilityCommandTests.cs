using HomeHive.Application.Features.Utilities.Commands.CreateUtility;

namespace HomeHive.Tests.ApplicationTests.Utilities.Commands.CreateCommand;

public class CreateUtilityCommandTests
{
    [Fact]
    public void CreateUtilityCommand_SetProperties_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var originalCommand = new CreateUtilityCommand("UtilityName");

        // Act
        var command = originalCommand with
        {
            UtilityName = "UpdatedUtilityName"
        };

        // Assert
        Assert.Equal("UpdatedUtilityName", command.UtilityName);
    }
}