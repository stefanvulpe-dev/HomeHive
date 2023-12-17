using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries.GetAllContracts;

public class GetAllContractsQueryHandlerTests
{
    private readonly IContractRepository _contractRepositoryMock;
    
    public GetAllContractsQueryHandlerTests()
    {
        _contractRepositoryMock = Substitute.For<IContractRepository>();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractRepositoryGetAllAsyncFails()
    {
        // Arrange
        _contractRepositoryMock.GetAllAsync().Returns(Result<IReadOnlyList<Contract>>.Failure("Failed to get contracts."));
        var handler = new GetAllContractsQueryHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(new GetAllContractsQuery(), CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Contracts not found.", result.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenContractRepositoryGetAllAsyncSucceeds()
    {
        // Arrange
        _contractRepositoryMock.GetAllAsync().Returns(Result<IReadOnlyList<Contract>>.Success(new List<Contract>()));
        var handler = new GetAllContractsQueryHandler(_contractRepositoryMock);
        
        // Act
        var result = await handler.Handle(new GetAllContractsQuery(), CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        foreach (var contract in result.Contracts!)
        {
            Assert.NotNull(contract.ContractType);
            Assert.NotNull(contract.Description);
            Assert.NotNull(contract.StartDate);
            Assert.NotNull(contract.EndDate);
        }
    }
}