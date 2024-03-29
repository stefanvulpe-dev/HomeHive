﻿using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries.GetAllContractsByUserId;

public class GetAllContractsByUserIdQueryHandlerTests
{
    private readonly IContractRepository _contractRepositoryMock;

    public GetAllContractsByUserIdQueryHandlerTests()
    {
        _contractRepositoryMock = Substitute.For<IContractRepository>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenContractRepositoryGetContractsByUserIdFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetAllContractsByUserIdQuery(userId);
        _contractRepositoryMock.GetContractsByUserId(userId)
            .Returns(Result<IReadOnlyList<Contract>>.Failure("Failed to get contracts."));
        var handler = new GetAllContractsByUserIdQueryHandler(_contractRepositoryMock);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Contracts not found for user with id: {request.UserId}.", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenContractRepositoryGetContractsByUserIdSucceeds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new GetAllContractsByUserIdQuery(userId);
        _contractRepositoryMock.GetContractsByUserId(userId)
            .Returns(Result<IReadOnlyList<Contract>>.Success(new List<Contract>()));
        var handler = new GetAllContractsByUserIdQueryHandler(_contractRepositoryMock);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

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

    [Fact]
    public void GetAllContractsByUserIdQuery_SetPropertiesWithInitializer_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var initialUserId = Guid.NewGuid();

        var getAllContractsQuery = new GetAllContractsByUserIdQuery(initialUserId);

        // Act
        var updatedUserId = Guid.NewGuid();

        getAllContractsQuery = getAllContractsQuery with
        {
            UserId = updatedUserId
        };

        // Assert
        Assert.Equal(updatedUserId, getAllContractsQuery.UserId);
    }
}