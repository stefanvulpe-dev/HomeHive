using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Domain.Entities;

public sealed class Room : BaseEntity
{
    private Room()
    {
    }
    
    public RoomType? RoomType { get; private set; }
    public int Capacity { get; private set; }
    public int Size { get; private set; }
    public ICollection<EstateRoom> EstateRooms { get; private set; }

    public static Result<Room> Create(RoomData roomData)
    {
        var (roomType, capacity, size) = roomData;

        if (string.IsNullOrWhiteSpace(roomType) || !Enum.TryParse(roomType, out RoomType typeEnum))
            return Result<Room>.Failure("RoomType is not valid.");

        if (capacity <= 0) return Result<Room>.Failure("Capacity should be greater than 0.");

        if (size <= 0) return Result<Room>.Failure("Size should be greater than 0.");
        
        return Result<Room>.Success(new Room
        {
            RoomType = Enum.Parse<RoomType>(roomType),
            Capacity = capacity,
            Size = size,
        });
    }
}