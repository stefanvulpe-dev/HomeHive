using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Domain.Entities;

public sealed class Room : BaseEntity
{
    private Room()
    {
    }

    public Guid EstateId { get; set; }
    public Estate? Estate { get; set; }
    public string? Name { get; set; }
    public RoomType? RoomType { get; set; }
    public int Capacity { get; set; }
    public int Size { get; set; }

    public static Result<Room> Create(RoomData roomData)
    {
        var (name, roomType, capacity, size, estate) = roomData;
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Room>.Failure("Name is not valid.");
        }

        if (string.IsNullOrWhiteSpace(roomType) || !Enum.TryParse(roomType, out RoomType typeEnum))
        {
            return Result<Room>.Failure("RoomType is not valid.");
        }

        if (capacity <= 0)
        {
            return Result<Room>.Failure("Capacity should be greater than 0.");
        }

        if (size <= 0)
        {
            return Result<Room>.Failure("Size should be greater than 0.");
        }

        if (estate == null)
        {
            return Result<Room>.Failure("Estate is required.");
        }

        return Result<Room>.Success(new Room
        {
            Name = name,
            RoomType = Enum.Parse<RoomType>(roomType),
            Capacity = capacity,
            Size = size,
            EstateId = estate.Id,
            Estate = estate
        });
    }
}