using HomeHive.Domain.Common;

namespace HomeHive.Tests.DomainTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResultWithoutMessageAndErrors()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Message);
        Assert.Null(result.ValidationErrors);
    }

    [Fact]
    public void Success_WithMessage_ShouldCreateSuccessfulResultWithMessageAndWithoutErrors()
    {
        // Arrange
        string message = "Operation succeeded";

        // Act
        var result = Result.Success(message);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(message, result.Message);
        Assert.Null(result.ValidationErrors);
    }

    [Fact]
    public void Failure_WithMessage_ShouldCreateFailureResultWithMessageAndWithoutErrors()
    {
        // Arrange
        string errorMessage = "Operation failed";

        // Act
        var result = Result.Failure(errorMessage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Message);
        Assert.Null(result.ValidationErrors);
    }

    [Fact]
    public void Failure_WithMessageAndErrors_ShouldCreateFailureResultWithMessageAndErrors()
    {
        // Arrange
        string errorMessage = "Operation failed";
        var errors = new Dictionary<string, string>
        {
            { "Field1", "Error 1" },
            { "Field2", "Error 2" }
        };

        // Act
        var result = Result.Failure(errorMessage, errors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Message);
        Assert.Equal(errors, result.ValidationErrors!);
    }
}