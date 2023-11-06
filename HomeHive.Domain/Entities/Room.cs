using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Room : BaseEntity
{
    private Room()
    {
    }

    public Guid EstateId { get; set; }
    public string? Name { get; set; }
    public RoomType? Type { get; set; }
    public int Capacity { get; set; }

    public int Size { get; set; }

    public static Result<Room> Create(string name, RoomType type, int capacity, int size, Guid estateId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Room>.Failure("Name is not valid.");
        }

        if (type == default)
        {
            return Result<Room>.Failure("Type should not be default.");
        }

        if (capacity <= 0)
        {
            return Result<Room>.Failure("Capacity should be greater than 0.");
        }

        if (size <= 0)
        {
            return Result<Room>.Failure("Size should be greater than 0.");
        }

        if (estateId == default)
        {
            return Result<Room>.Failure("Estate ID should not be default.");
        }

        return Result<Room>.Success(new Room
        {
            Name = name,
            Type = type,
            Capacity = capacity,
            Size = size,
            EstateId = estateId
        });
    }
}

public enum RoomType
{
    LivingRoom,
    Bedroom,
    Kitchen,
    Bathroom,
    Balcony,
}