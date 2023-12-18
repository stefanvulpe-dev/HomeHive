using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Tests.DomainTests.EntitiesUtils;

public class ContractDataTests
{
    [Fact]
    public void ContractData_ParameterizedConstructor_ShouldSetProperties()
    {
        // Arrange
        var estateId = Guid.NewGuid();
        var contractType = "Rent";
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddMonths(6);
        var description = "Lease agreement";

        // Act
        var contractData = new ContractData(estateId, contractType, startDate, endDate, description);

        // Assert
        Assert.Equal(estateId, contractData.EstateId);
        Assert.Equal(contractType, contractData.ContractType);
        Assert.Equal(startDate, contractData.StartDate);
        Assert.Equal(endDate, contractData.EndDate);
        Assert.Equal(description, contractData.Description);
    }

    [Fact]
    public void ContractData_SetProperties_ShouldModifyProperties()
    {
        // Arrange
        var contractData = new ContractData(Guid.Empty, null, null, null, null);

        // Act
        contractData = contractData with { EstateId = Guid.NewGuid(), ContractType = "Rent" };

        // Assert
        Assert.NotEqual(Guid.Empty, contractData.EstateId);
        Assert.Equal("Rent", contractData.ContractType);
    }
}