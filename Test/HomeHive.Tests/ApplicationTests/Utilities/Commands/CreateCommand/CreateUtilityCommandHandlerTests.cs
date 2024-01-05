using HomeHive.Application.Features.Utilities.Commands.CreateUtility;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Utilities.Commands.CreateCommand;

public class CreateUtilityCommandHandlerTests
{
    private readonly IUtilityRepository _utilityRepository;

    public CreateUtilityCommandHandlerTests()
    {
        _utilityRepository = Substitute.For<IUtilityRepository>();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityNameIsNotUnique()
    {
        // Arrange
        var utilityName = "Gas";
        var request = new CreateUtilityCommand(utilityName);

        _utilityRepository.GetAllAsync()
            .Returns(Result<IReadOnlyList<Utility>>.Success(new List<Utility> { Utility.Create(utilityName).Value }));
        var handler = new CreateUtilityCommandHandler(_utilityRepository);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("A utility with the same name already exists.", result.ValidationsErrors!["UtilityName"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityNameIsEmpty()
    {
        // Arrange
        var request = new CreateUtilityCommand("");
        var handler = new CreateUtilityCommandHandler(_utilityRepository);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Utility Name is required.", result.ValidationsErrors!["UtilityName"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityNameIsTooLong()
    {
        // Arrange
        var request = new CreateUtilityCommand(new string('a', 51));
        var handler = new CreateUtilityCommandHandler(_utilityRepository);
        _utilityRepository.GetAllAsync().Returns(Result<IReadOnlyList<Utility>>.Success(new List<Utility>()));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Utility Name must not exceed 50 characters.", result.ValidationsErrors!["UtilityName"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResponse_WhenUtilityNameIsValid()
    {
        // Arrange
        var utilityName = "Gas";
        var request = new CreateUtilityCommand(utilityName);

        _utilityRepository.GetAllAsync().Returns(Result<IReadOnlyList<Utility>>.Success(new List<Utility>()));
        var handler = new CreateUtilityCommandHandler(_utilityRepository);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(utilityName, result.Utility.UtilityName);
    }
}