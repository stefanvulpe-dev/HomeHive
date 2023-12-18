using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Tests.DomainTests.EntitiesUtils;

public class EstateDataTests
{
    [Fact]
    public void EstateData_ParameterizedConstructor_ShouldSetProperties()
    {
        // Arrange
        string estateType = "Residential";
        string estateCategory = "Apartment";
        string name = "Luxury Apartment";
        string location = "City Center";
        decimal price = 1000000.50m;
        string totalArea = "150 sqm";
        List<string> utilities = ["Water", "Electricity"];
        Dictionary<string, int> estateRooms = new Dictionary<string, int>(){ {"Bedroom", 2 }, {"Bathroom", 1} };
        string description = "Modern apartment with great amenities";
        string image = "image-url.jpg";

        // Act
        var estateData = new EstateData(
            estateType, estateCategory, name, location, price, totalArea, utilities, estateRooms, description, image);

        // Assert
        Assert.Equal(estateType, estateData.EstateType);
        Assert.Equal(estateCategory, estateData.EstateCategory);
        Assert.Equal(name, estateData.Name);
        Assert.Equal(location, estateData.Location);
        Assert.Equal(price, estateData.Price);
        Assert.Equal(totalArea, estateData.TotalArea);
        Assert.Equal(utilities, estateData.Utilities);
        Assert.Equal(description, estateData.Description);
        Assert.Equal(image, estateData.EstateAvatar);
    }

    [Fact]
    public void EstateData_SetProperties_ShouldModifyProperties()
    {
        // Arrange
        var estateData = new EstateData(null, null, null, null, null, null, null, null, null, null);

        // Act
        estateData = estateData with { EstateType = "House", Price = 500000.75m };

        // Assert
        Assert.Equal("House", estateData.EstateType);
        Assert.Equal(500000.75m, estateData.Price);
    }
}