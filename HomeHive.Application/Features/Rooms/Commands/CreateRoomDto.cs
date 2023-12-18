

namespace HomeHive.Application.Features.Rooms.Commands;

public class CreateRoomDto
{
    public Guid Id { get; set; }
    public Guid EstateId { get; set; }
    public string? RoomType { get; set; }
    public int Quantity { get; set; }
}