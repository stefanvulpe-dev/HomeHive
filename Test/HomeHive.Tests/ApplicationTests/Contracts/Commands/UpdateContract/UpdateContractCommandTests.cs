using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.UpdateContract;

public class UpdateContractCommandTests
{
    [Fact]
    public async Task Handle_ValidUpdateContractCommand_ShouldReturnSuccess()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contractData = new ContractData(Guid.NewGuid(), "Rent", DateTime.Now, DateTime.Now.AddMonths(6),
            "Updated Description");
        var updateCommand = new UpdateContractCommand(contractId, contractData);

        var contractRepositoryMock = Substitute.For<IContractRepository>();
        var contractResult = Contract.Create(Guid.NewGuid(), contractData);

        contractRepositoryMock.FindByIdAsync(contractId)
            .Returns(Result<Contract>.Success(contractResult.Value));

        var handler = new UpdateContractCommandHandler(contractRepositoryMock);

        // Act
        var result = await handler.Handle(updateCommand, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.ValidationsErrors);
    }

    [Fact]
    public async Task Handle_InvalidContractId_ShouldReturnFailure()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var contractData = new ContractData(Guid.NewGuid(), "Rent", DateTime.Now, DateTime.Now.AddMonths(6),
            "Updated Description");
        var updateCommand = new UpdateContractCommand(contractId, contractData);

        var contractRepositoryMock = Substitute.For<IContractRepository>();

        contractRepositoryMock.FindByIdAsync(contractId)
            .Returns(Result<Contract>.Failure("Contract not found."));

        var handler = new UpdateContractCommandHandler(contractRepositoryMock);

        // Act
        var result = await handler.Handle(updateCommand, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Contract with {contractId} does not exist", result.Message);
    }

    [Fact]
    public void UpdateContractCommand_SetProperties_ShouldModifyPropertyValues()
    {
        // Arrange
        var originalCommand = new UpdateContractCommand(Guid.NewGuid(),
            new ContractData(Guid.NewGuid(), ContractType.Rent.ToString(), DateTime.Now, DateTime.Now.AddMonths(6),
                "description"));

        // Act
        var modifiedCommand = originalCommand with
        {
            Data = originalCommand.Data with
            {
                Description = "modified description"
            }
        };

        // Assert
        Assert.Equal("modified description", modifiedCommand.Data.Description);
    }
}