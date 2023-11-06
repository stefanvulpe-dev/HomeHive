using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Photo : BaseEntity
{
    private Photo()
    {
    }

    public Guid EstateId { get; set; }
    public string? ObjectName { get; set; }

    public static Result<Photo> Create(string objectName, Guid estateId)
    {
        if (string.IsNullOrWhiteSpace(objectName))
        {
            return Result<Photo>.Failure("Object name is not valid!");
        }

        if (estateId == default)
        {
            return Result<Photo>.Failure("Estate id should not be default!");
        }

        return Result<Photo>.Success(new Photo { EstateId = estateId, ObjectName = objectName });
    }
}