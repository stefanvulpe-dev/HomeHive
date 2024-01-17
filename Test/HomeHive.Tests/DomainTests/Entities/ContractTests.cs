using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;

namespace HomeHive.Tests.DomainTests.Entities;

public class ContractTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEstateIdIsEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var contractData = new ContractData(Guid.Empty, Guid.Empty,
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");
        // Act
        var result = Contract.Create(userId, contractData);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("EstateId is required.", result.Message);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenUserIdIsEmpty()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var contractData = new ContractData(estateId, Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");
        // Act
        var result = Contract.Create(Guid.Empty, contractData);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("UserId is required.", result.Message);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenContractTypeIsNotValid()
    {
        // Arrange
        var (estateId, userId) = (Guid.NewGuid(), Guid.NewGuid());
        var contractData = new ContractData(estateId, Guid.NewGuid(),
            "",
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");
        // Act
        var result = Contract.Create(userId, contractData);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Type is not valid.", result.Message);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenStartDateIsDefault()
    {
        // Arrange
        var (estateId, userId) = (Guid.NewGuid(), Guid.NewGuid());
        var contractData = new ContractData(estateId,
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            default,
            DateTime.Now,
            "Test");
        // Act
        var result = Contract.Create(userId, contractData);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Start date should not be default!", result.Message);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenDescriptionIsNotValid()
    {
        // Arrange
        var (estateId, userId) = (Guid.NewGuid(), Guid.NewGuid());
        var contractData = new ContractData(estateId,
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "");
        // Act
        var result = Contract.Create(userId, contractData);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Description is not valid!", result.Message);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var contractData = new ContractData(Guid.NewGuid(),
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");
        var userId = Guid.NewGuid();
        // Act
        var result = Contract.Create(userId, contractData);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(contractData.EstateId, result.Value.EstateId);
        Assert.Equal(contractData.ContractType, result.Value.ContractType.ToString());
        Assert.Equal(contractData.StartDate, result.Value.StartDate);
        Assert.Equal(contractData.EndDate, result.Value.EndDate);
        Assert.Equal(contractData.Description, result.Value.Description);
        Assert.Null(result.Value.Estate);
    }

    [Fact]
    public void Update_ShouldUpdateContract_WhenDataIsValid()
    {
        // Arrange
        var contractData = new ContractData(Guid.NewGuid(),
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "Test");
        var contract = Contract.Create(Guid.NewGuid(), contractData).Value;
        var newContractData = new ContractData(Guid.NewGuid(),
            Guid.NewGuid(),
            ContractType.Rent.ToString(),
            ContractStatus.Done.ToString(),
            200000,
            DateTime.Now,
            DateTime.Now,
            "New Test");

        // Act
        contract.Update(newContractData);

        // Assert
        Assert.Equal(newContractData.EstateId, contract.EstateId);
        Assert.Equal(newContractData.ContractType, contract.ContractType.ToString());
        Assert.Equal(newContractData.StartDate, contract.StartDate);
        Assert.Equal(newContractData.EndDate, contract.EndDate);
        Assert.Equal(newContractData.Description, contract.Description);
    }
}