using HomeHive.Application.Features.Contracts.Queries.GetAllContracts;

namespace HomeHive.Tests.ApplicationTests.Contracts.Queries.GetAllContracts;

public class GetAllQueriesTests
{
    [Fact]
    public void GetAllContractsQuery_SetPropertiesWithInitializer_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var getAllContractsQuery = new GetAllContractsQuery();

        // Act
        var updateGetAllContractsQuery = getAllContractsQuery with
        {
        };

        // Assert
        Assert.Equal(getAllContractsQuery, updateGetAllContractsQuery);
    }
}