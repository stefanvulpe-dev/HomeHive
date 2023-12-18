using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Domain.Entities;

public sealed class Room : BaseEntity
{
    private Room()
    {
    }
    
    public RoomType? RoomType { get; private set; }

    public ICollection<EstateRoom> EstateRooms { get; private set; }

    public static Result<Room> Create(string? roomType)
    {

        if (string.IsNullOrWhiteSpace(roomType) || !Enum.TryParse(roomType, out RoomType typeEnum))
            return Result<Room>.Failure("RoomType is not valid.");
        
        return Result<Room>.Success(new Room
        {
            RoomType = Enum.Parse<RoomType>(roomType),
        });
    }
}