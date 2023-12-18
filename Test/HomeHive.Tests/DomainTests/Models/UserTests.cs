using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Models;
using NSubstitute;

namespace HomeHive.Tests.DomainTests.Models;

public class UserTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    
    [Fact]
        public void User_Properties_ShouldHaveCorrectGettersAndSetters()
        {
            // Arrange
            var user = new User();
            
            _userRepository.FindByIdAsync(user.Id).Returns(Result<User>.Success(user));

            // Act and Assert
            user.FirstName = "John";
            Assert.Equal("John", user.FirstName);
            
            user.LastName = "Doe";
            Assert.Equal("Doe", user.LastName);
            
            user.ProfilePicture = "image.png";
            Assert.Equal("image.png", user.ProfilePicture);
            
            user.CreatedDate = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, user.CreatedDate.Kind);
            
            user.LastModifiedDate = DateTime.UtcNow;
            Assert.Equal(DateTimeKind.Utc, user.LastModifiedDate.Kind);
            
            Assert.Null(user.Estates);
        }
}
