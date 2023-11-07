using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Photo : BaseEntity
{
    private Photo()
    {
    }

    public Guid EstateId { get; set; }
    public Estate? Estate { get; set; }
    public string? ObjectName { get; set; }

    public static Result<Photo> Create(string? objectName, Estate? estate)
    {
        if (string.IsNullOrWhiteSpace(objectName))
        {
            return Result<Photo>.Failure("Object name is not valid!");
        }

        if (estate == null)
        {
            return Result<Photo>.Failure("Estate is required.");
        }

        return Result<Photo>.Success(new Photo { EstateId = estate.Id, Estate = estate, ObjectName = objectName });
    }
}