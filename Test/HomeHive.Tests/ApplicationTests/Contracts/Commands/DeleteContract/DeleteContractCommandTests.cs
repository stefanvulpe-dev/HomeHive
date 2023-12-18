using HomeHive.Application.Features.Contracts.Commands.DeleteContractById;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.DeleteContract;

public class DeleteContractCommandTests
{
    [Fact]
    public void DeleteContractByIdCommand_SetProperties_ShouldModifyPropertyValues()
    {
        // Arrange
        var originalCommand = new DeleteContractByIdCommand(Guid.NewGuid());

        // Act
        var command = originalCommand with
        {
            ContractId = Guid.NewGuid()
        };

        // Assert
        Assert.NotEqual(originalCommand.ContractId, command.ContractId);
    }
}