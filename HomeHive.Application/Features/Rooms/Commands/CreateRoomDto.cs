

namespace HomeHive.Application.Features.Rooms.Commands;

public class CreateRoomDto
{
    public Guid Id { get; set; }
    public string? RoomType { get; set; }
}