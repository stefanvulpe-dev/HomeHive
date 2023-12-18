using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries.GetAllContractsByUserId;

public class UserContractDtoTests
{
    [Fact]
    public void UserContractDto_Initialization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var contractType = "Rent";
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(30);
        var description = "Test Description";

        // Act
        var userContractDto = new UserContractDto
        {
            EstateId = estateId,
            ContractType = contractType,
            StartDate = startDate,
            EndDate = endDate,
            Description = description
        };

        // Assert
        Assert.Equal(estateId, userContractDto.EstateId);
        Assert.Equal(contractType, userContractDto.ContractType);
        Assert.Equal(startDate, userContractDto.StartDate);
        Assert.Equal(endDate, userContractDto.EndDate);
        Assert.Equal(description, userContractDto.Description);
    }

    [Fact]
    public void UserContractDto_SetPropertiesWithInitializer_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var userContractDto = new UserContractDto
        {
            EstateId = Guid.NewGuid(),
            ContractType = "Rent",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            Description = "Test Description"
        };

        // Act
        var updatedEstateId = Guid.NewGuid();
        var updatedDescription = "Updated Description";

        var updateUser = userContractDto with
        {
            EstateId = updatedEstateId,
            Description = updatedDescription
        };

        // Assert
        Assert.Equal(updatedEstateId, updateUser.EstateId);
        Assert.Equal(updatedDescription, updateUser.Description);
    }
}