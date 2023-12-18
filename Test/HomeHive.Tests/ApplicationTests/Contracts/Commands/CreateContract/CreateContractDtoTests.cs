using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Tests.ApplicationTests.Contracts.Commands.CreateContract;

public class CreateContractDtoTests
{
    [Fact]
    public void CreateContractDto_GetProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var contractId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var estateId = Guid.NewGuid();
        var contractType = ContractType.Rent;
        var description = "Test Description";
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(30);

        // Act
        var createContractDto = new CreateContractDto
        {
            ContractId = contractId,
            UserId = userId,
            EstateId = estateId,
            ContractType = contractType,
            Description = description,
            StartDate = startDate,
            EndDate = endDate
        };

        // Assert
        Assert.Equal(contractId, createContractDto.ContractId);
        Assert.Equal(userId, createContractDto.UserId);
        Assert.Equal(estateId, createContractDto.EstateId);
        Assert.Equal(contractType, createContractDto.ContractType);
        Assert.Equal(description, createContractDto.Description);
        Assert.Equal(startDate, createContractDto.StartDate);
        Assert.Equal(endDate, createContractDto.EndDate);
    }
}