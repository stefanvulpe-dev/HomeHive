using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.UpdateContract;

public class UpdateContractCommandHandlerTests
{
    private readonly IContractRepository _contractRepositoryMock;

    public UpdateContractCommandHandlerTests()
    {
        _contractRepositoryMock = Substitute.For<IContractRepository>();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractIdIsEmpty()
    {
        // Arrange
        ContractData contractData = new ContractData(
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            DateTime.Now,
            DateTime.Now.AddDays(30),
            "Test");
        
        var command = new UpdateContractCommand(Guid.Empty, contractData);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update contract.", result.Message);
        Assert.Contains("ContractId is required.", result.ValidationsErrors!["Id"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractDataIsNull()
    {
        // Arrange
        var command = new UpdateContractCommand(Guid.NewGuid(), null!);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update contract.", result.Message);
        Assert.Contains("ContractData is required.", result.ValidationsErrors!["Data"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractDataHasNoValues()
    {
        // Arrange
        ContractData contractData = new ContractData(
            Guid.Empty, null, null, null, null);
        
        var command = new UpdateContractCommand(Guid.NewGuid(), contractData);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update contract.", result.Message);
        Assert.Contains("At least one property of ContractData must be filled.", result.ValidationsErrors!["Data"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractDataHasInvalidContractType()
    {
        // Arrange
        ContractData contractData = new ContractData(
            Guid.NewGuid(),
            "InvalidContractType",
            DateTime.Now,
            DateTime.Now.AddDays(30),
            "Test");
        
        var command = new UpdateContractCommand(Guid.NewGuid(), contractData);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update contract.", result.Message);
        Assert.Contains("ContractType is not valid.", result.ValidationsErrors!["Data"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractDoesNotExist()
    {
        // Arrange
        ContractData contractData = new ContractData(
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            DateTime.Now,
            DateTime.Now.AddDays(30),
            "Test");
        Guid contractId = Guid.NewGuid();
        
        _contractRepositoryMock.FindByIdAsync(contractId).Returns(Result<Contract>.Failure("Contract does not exist."));
        
        var command = new UpdateContractCommand(contractId, contractData);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Contract with {command.Id} does not exist", result.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenContractIdIsValidAndContractDataIsValid()
    {
        // Arrange
        ContractData contractData = new ContractData(
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            DateTime.Now,
            DateTime.Now.AddDays(30),
            "Test");
        
        Result<Contract> contractResult = Contract.Create(Guid.NewGuid(), contractData);
        
        _contractRepositoryMock.FindByIdAsync(contractResult.Value.Id)
            .Returns(Result<Contract>.Success(contractResult.Value));
        _contractRepositoryMock.UpdateAsync(contractResult.Value)
            .Returns(Result<Contract>.Success(contractResult.Value));
        
        var command = new UpdateContractCommand(contractResult.Value.Id, contractData);
        var handler = new UpdateContractCommandHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Contract!.EstateId, contractData.EstateId);
        Assert.Equal(result.Contract!.UserId, contractResult.Value.UserId);
        Assert.Equal(result.Contract!.ContractType.ToString(), contractData.ContractType);
        Assert.Equal(result.Contract!.StartDate, contractData.StartDate);
        Assert.Equal(result.Contract!.EndDate, contractData.EndDate);
        Assert.Equal(result.Contract!.Description, contractData.Description);
    }
}