using HomeHive.Domain.Common;

namespace HomeHive.Tests.DomainTests.Common;

public class LoginResultTests
{
    [Fact]
    public void LoginResult_Success_ShouldCreateInstanceWithAccessTokenAndRefreshToken()
    {
        // Arrange
        string successMessage = "Login successful";
        string accessToken = "fakeAccessToken";
        string refreshToken = "fakeRefreshToken";

        // Act
        var loginResult = LoginResult.Success(successMessage, accessToken, refreshToken);

        // Assert
        Assert.True(loginResult.IsSuccess);
        Assert.Equal(successMessage, loginResult.Message);
        Assert.Equal(accessToken, loginResult.AccessToken);
        Assert.Equal(refreshToken, loginResult.RefreshToken);
        Assert.Null(loginResult.Errors);
        
    }

    [Fact]
    public void LoginResult_FailureWithErrors_ShouldCreateInstanceWithErrors()
    {
        // Arrange
        string failureMessage = "Login failed";
        var errors = new Dictionary<string, string?>
        {
            { "Username", "Invalid username" },
            { "Password", "Invalid password" }
        };

        // Act
        var loginResult = LoginResult.Failure(failureMessage, errors);

        // Assert
        Assert.False(loginResult.IsSuccess);
        Assert.Equal(failureMessage, loginResult.Message);
        Assert.Null(loginResult.AccessToken);
        Assert.Null(loginResult.RefreshToken);
        Assert.NotNull(loginResult.Errors);
        Assert.Equal(errors, loginResult.Errors);
    }

    [Fact]
    public void LoginResult_FailureWithoutErrors_ShouldCreateInstanceWithEmptyErrors()
    {
        // Arrange
        string failureMessage = "Login failed";

        // Act
        var loginResult = LoginResult.Failure(failureMessage);

        // Assert
        Assert.False(loginResult.IsSuccess);
        Assert.Equal(failureMessage, loginResult.Message);
        Assert.Null(loginResult.AccessToken);
        Assert.Null(loginResult.RefreshToken);
        Assert.Null(loginResult.Errors);
    }
}