using HomeHive.Application.Features.Contracts.Queries.GetContractById;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries.GetContractById;

public class GetContractByIdQueryHandlerTests
{
    private readonly IContractRepository _contractRepositoryMock;

    public GetContractByIdQueryHandlerTests()
    {
        _contractRepositoryMock = Substitute.For<IContractRepository>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractRepositoryGetByIdAsyncFails()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var request = new GetContractByIdQuery(contractId);
        _contractRepositoryMock.FindByIdAsync(contractId).Returns(Result<Contract>.Failure("Failed to get contract."));
        var handler = new GetContractByIdQueryHandler(_contractRepositoryMock);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Contract not found.", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenContractRepositoryGetByIdAsyncSucceeds()
    {
        // Arrange
        var contractData = new ContractData(Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");

        var contractResult = Contract.Create(Guid.NewGuid(), contractData);

        var request = new GetContractByIdQuery(contractResult.Value.Id);

        _contractRepositoryMock.FindByIdAsync(contractResult.Value.Id)
            .Returns(Result<Contract>.Success(contractResult.Value));

        var handler = new GetContractByIdQueryHandler(_contractRepositoryMock);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Contract);
    }
}