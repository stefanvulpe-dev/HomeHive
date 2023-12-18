using HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

namespace HomeHive.Tests.ApplicationTests.Utilities.Commands.UpdateCommand;

public class UpdateUtilityCommandTests
{
    [Fact]
    public void UpdateUtilityCommand_SetProperties_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var originalCommand = new UpdateUtilityCommand(Guid.NewGuid(), "UtilityName");

        // Act
        var command = originalCommand with
        {
            UtilityName = "UpdatedUtilityName"
        };

        // Assert
        Assert.Equal("UpdatedUtilityName", command.UtilityName);
    }
}