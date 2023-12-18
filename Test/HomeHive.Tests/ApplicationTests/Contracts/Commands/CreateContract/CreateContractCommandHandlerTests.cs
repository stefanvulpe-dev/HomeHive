using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.CreateContract;

public class CreateContractCommandHandlerTests
{
    private static readonly Guid OwnerId = Guid.NewGuid();

    private static readonly EstateData EstateData = new(
        EstateType.Apartment.ToString(),
        EstateCategory.ForSale.ToString(),
        "Test",
        "Test",
        100,
        "Test",
        ["Test"],
        "Test",
        "Test");

    private static readonly List<Utility> Utilities = new() { Utility.Create("Test Utilities").Value };
    private readonly IContractRepository _contractRepositoryMock;
    private readonly IEstateRepository _estateRepositoryMock;

    public CreateContractCommandHandlerTests()
    {
        _contractRepositoryMock = Substitute.For<IContractRepository>();
        _estateRepositoryMock = Substitute.For<IEstateRepository>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEstateDoesNotExist()
    {
        // Arrange
        var command = new CreateContractCommand(Guid.NewGuid(),
            new ContractData(Guid.NewGuid(),
                ContractType.Rent.ToString(),
                DateTime.Now,
                DateTime.Now,
                "Test"));
        _estateRepositoryMock.FindByIdAsync(Arg.Any<Guid>()).Returns(Result<Estate>.Failure("Estate does not exist."));
        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Estate does not exist.", result.ValidationsErrors!["Data.EstateId"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEstateIdIsEmpty()
    {
        // Arrange
        var command = new CreateContractCommand(Guid.NewGuid(),
            new ContractData(Guid.Empty,
                ContractType.Rent.ToString(),
                DateTime.Now,
                DateTime.Now,
                "Test"));
        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Estate Id is required.", result.ValidationsErrors!["Data.EstateId"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractDescriptionIsEmpty()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, "Rent", DateTime.Now, DateTime.Now, string.Empty)
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Description is required.", result.ValidationsErrors!["Data.Description"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractDescriptionIsNull()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, "Rent", DateTime.Now, DateTime.Now, null)
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Description can't be null.", result.ValidationsErrors!["Data.Description"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractDescriptionIsTooLong()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var longDescription = new string('A', 201);

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, "Rent", DateTime.Now, DateTime.Now, longDescription)
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Description must not exceed 200 characters.",
            result.ValidationsErrors!["Data.Description"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractStartDateIsNull()
    {
        // Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, "Rent", null, DateTime.Now, "Test")
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Start Date can't be null.", result.ValidationsErrors!["Data.StartDate"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractEndDateIsNull()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, "Rent", DateTime.Now, null, "Test")
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data End Date can't be null.", result.ValidationsErrors!["Data.EndDate"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractTypeIsEmpty()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, string.Empty, DateTime.Now, DateTime.Now, "Test")
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Contract Type is required.", result.ValidationsErrors!["Data.ContractType"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContractTypeIsNull()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, null, DateTime.Now, DateTime.Now, "Test")
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create contract.", result.Message);
        Assert.Contains("Data Contract Type can't be null.", result.ValidationsErrors!["Data.ContractType"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenContractIsValid()
    {
        //Arrange
        var estateCreationResult = Estate.Create(OwnerId, Utilities, EstateData);

        _estateRepositoryMock.FindByIdAsync(estateCreationResult.Value.Id)
            .Returns(Result<Estate>.Success(estateCreationResult.Value));

        var command = new CreateContractCommand(
            Guid.NewGuid(),
            new ContractData(estateCreationResult.Value.Id, ContractType.Rent.ToString(), DateTime.Now, DateTime.Now,
                "Test")
        );

        var handler = new CreateContractCommandHandler(_contractRepositoryMock, _estateRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Contract created successfully.", result.Message);
        Assert.Equal(estateCreationResult.Value.Id, result.Contract!.EstateId);
        Assert.Equal(command.UserId, result.Contract.UserId);
        Assert.Equal(command.Data.ContractType, result.Contract.ContractType.ToString());
        Assert.Equal(command.Data.StartDate, result.Contract.StartDate);
        Assert.Equal(command.Data.EndDate, result.Contract.EndDate);
        Assert.Equal(command.Data.Description, result.Contract.Description);
    }
}