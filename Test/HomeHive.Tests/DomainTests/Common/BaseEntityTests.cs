using HomeHive.Domain.Common;

namespace HomeHive.Tests.DomainTests.Common;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_DefaultValues_ShouldBeSetCorrectly()
    {
        // Act
        var baseEntity = new BaseEntity();

        // Assert
        Assert.NotEqual(Guid.Empty, baseEntity.Id);
        Assert.Null(baseEntity.CreatedBy);
        Assert.NotEqual(default(DateTime), baseEntity.CreatedDate); 
        Assert.Null(baseEntity.LastModifiedBy);
        Assert.NotEqual(default(DateTime), baseEntity.LastModifiedDate);
    }

    [Fact]
    public void BaseEntity_SetValues_ShouldBeSetCorrectly()
    {
        // Arrange
        Guid customId = Guid.NewGuid();
        string createdBy = "JohnDoe";
        DateTime customCreatedDate = new DateTime(2022, 1, 1, 12, 0, 0);
        string lastModifiedBy = "JaneDoe";
        DateTime customLastModifiedDate = new DateTime(2022, 2, 1, 12, 0, 0);

        // Act
        var baseEntity = new BaseEntity
        {
            Id = customId,
            CreatedBy = createdBy,
            CreatedDate = customCreatedDate,
            LastModifiedBy = lastModifiedBy,
            LastModifiedDate = customLastModifiedDate
        };

        // Assert
        Assert.Equal(customId, baseEntity.Id);
        Assert.Equal(createdBy, baseEntity.CreatedBy);
        Assert.Equal(customCreatedDate, baseEntity.CreatedDate);
        Assert.Equal(lastModifiedBy, baseEntity.LastModifiedBy);
        Assert.Equal(customLastModifiedDate, baseEntity.LastModifiedDate);
    }
}