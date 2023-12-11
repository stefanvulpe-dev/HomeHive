using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Photo : BaseEntity
{
    private Photo()
    {
    }

    public Guid EstateId { get; private set; }
    public Estate? Estate { get; private set; }
    public string? ObjectName { get; private set; }

    public static Result<Photo> Create(string? objectName, Estate? estate)
    {
        if (string.IsNullOrWhiteSpace(objectName)) return Result<Photo>.Failure("Object name is not valid!");

        if (estate == null) return Result<Photo>.Failure("Estate is required.");

        return Result<Photo>.Success(new Photo { EstateId = estate.Id, Estate = estate, ObjectName = objectName });
    }
}