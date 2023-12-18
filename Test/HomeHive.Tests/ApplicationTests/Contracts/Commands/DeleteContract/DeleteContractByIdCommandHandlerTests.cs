using HomeHive.Application.Features.Contracts.Commands.DeleteContractById;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.DeleteContract;

public class DeleteContractByIdCommandHandlerTests
{
    private readonly IContractRepository _contractRepository;

    public DeleteContractByIdCommandHandlerTests()
    {
        _contractRepository = Substitute.For<IContractRepository>();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationErrors()
    {
        // Arrange
        var invalidCommand = new DeleteContractByIdCommand(Guid.Empty);

        var handler = new DeleteContractByIdCommandHandler(_contractRepository);

        // Act
        var result = await handler.Handle(invalidCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to delete contract.", result.Message);
        Assert.NotNull(result.ValidationsErrors);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractIdIsEmpty()
    {
        // Arrange
        var command = new DeleteContractByIdCommand(Guid.Empty);
        var handler = new DeleteContractByIdCommandHandler(_contractRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to delete contract.", result.Message);
        Assert.Contains("Contract Id is required.", result.ValidationsErrors!["ContractId"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractDoesNotExist()
    {
        // Arrange
        var command = new DeleteContractByIdCommand(Guid.NewGuid());
        _contractRepository.FindByIdAsync(command.ContractId)
            .Returns(Result<Contract>.Failure("Contract does not exist."));
        var handler = new DeleteContractByIdCommandHandler(_contractRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to delete contract.", result.Message);
        Assert.Contains($"Contract with Id {command.ContractId} does not exist.",
            result.ValidationsErrors!["ContractId"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractRepositoryDeleteByIdAsyncFails()
    {
        // Arrange
        var contractData = new ContractData(
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            DateTime.Now,
            DateTime.Now.AddDays(30),
            "Contract description");

        var contractResult = Contract.Create(Guid.NewGuid(), contractData);

        _contractRepository.FindByIdAsync(contractResult.Value.Id)
            .Returns(Result<Contract>.Success(contractResult.Value));
        _contractRepository.DeleteByIdAsync(contractResult.Value.Id)
            .Returns(Result.Failure("Error deleting contract."));

        var command = new DeleteContractByIdCommand(contractResult.Value.Id);
        var handler = new DeleteContractByIdCommandHandler(_contractRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Error deleting contract with Id {command.ContractId}", result.Message);
    }
}