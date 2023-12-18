using HomeHive.Application.Features.Contracts.Queries;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries;

public class ContractDtoTests
{
    [Fact]
    public void ContractDto_Initialization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var estateId = Guid.NewGuid();
        var contractType = "Rent";
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(30);
        var description = "Test Description";

        // Act
        var contractDto = new ContractDto
        {
            UserId = userId,
            EstateId = estateId,
            ContractType = contractType,
            StartDate = startDate,
            EndDate = endDate,
            Description = description
        };

        // Assert
        Assert.Equal(userId, contractDto.UserId);
        Assert.Equal(estateId, contractDto.EstateId);
        Assert.Equal(contractType, contractDto.ContractType);
        Assert.Equal(startDate, contractDto.StartDate);
        Assert.Equal(endDate, contractDto.EndDate);
        Assert.Equal(description, contractDto.Description);
    }
    
    [Fact]
    public void ContractDto_SetProperties_ShouldModifyPropertyValues()
    {
        // Arrange
        var originalContractDto = new ContractDto
        {
            UserId = Guid.NewGuid(),
            EstateId = Guid.NewGuid(),
            ContractType = "Rent",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            Description = "Test Description"
        };

        // Act
        var contractDto = originalContractDto with
        {
            UserId = Guid.NewGuid()
        };

        // Assert
        Assert.NotEqual(originalContractDto.UserId, contractDto.UserId);
    }
}