using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class EstatePhoto : BaseEntity
{
    private EstatePhoto()
    {
    }

    public Guid EstateId { get; private set; }
    public Estate? Estate { get; private set; }
    public string? ObjectName { get; private set; }

    public static Result<EstatePhoto> Create(string? objectName, Estate? estate)
    {
        if (string.IsNullOrWhiteSpace(objectName)) return Result<EstatePhoto>.Failure("Object name is not valid!");

        if (estate == null) return Result<EstatePhoto>.Failure("Estate is required.");

        return Result<EstatePhoto>.Success(new EstatePhoto
            { EstateId = estate.Id, Estate = estate, ObjectName = objectName });
    }
}