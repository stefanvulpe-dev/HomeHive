using HomeHive.Application.Features.Utilities.Commands.UpdateUtility;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using NSubstitute;

namespace HomeHive.Tests.ApplicationTests.Utilities.Commands.UpdateCommand;

public class UpdateUtilityCommandHandlerTests
{
    private readonly IUtilityRepository _utilityRepository;
    
    public UpdateUtilityCommandHandlerTests()
    {
        _utilityRepository = Substitute.For<IUtilityRepository>();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityIdIsInvalid()
    {
        // Arrange
        var command = new UpdateUtilityCommand(Guid.Empty, "UtilityName");
        var handler = new UpdateUtilityCommandHandler(_utilityRepository);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Utility Id is required.", result.ValidationsErrors!["UtilityId"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityNameIsInvalid()
    {
        // Arrange
        var command = new UpdateUtilityCommand(Guid.NewGuid(), "");
        var handler = new UpdateUtilityCommandHandler(_utilityRepository);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Utility Name is required.", result.ValidationsErrors!["UtilityName"]);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityDoesNotExist()
    {
        // Arrange
        var utilityId = Guid.NewGuid();
        var command = new UpdateUtilityCommand(utilityId, "UtilityName");
        var handler = new UpdateUtilityCommandHandler(_utilityRepository);

        _utilityRepository.FindByIdAsync(utilityId)
            .Returns(Result<Utility>.Failure("Utility not found"));
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Utility {command.UtilityId} not found", result.Message);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailureResponse_WhenUtilityIsUpdated()
    {
        // Arrange
        var utility = Utility.Create("UtilityName").Value;
        
        var command = new UpdateUtilityCommand(utility.Id, "UtilityName");
        var handler = new UpdateUtilityCommandHandler(_utilityRepository);
        
        _utilityRepository.FindByIdAsync(utility.Id)
            .Returns(Result<Utility>.Success(utility));
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Utility updated successfully", result.Message);
        Assert.Equal(utility.Id, result.Utility!.Id);
        Assert.Equal("UtilityName", result.Utility.UtilityName);
    }
}