using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Rooms.Queries.GetAllRooms;

public class GetAllRoomsResponse: BaseResponse
{
    public IReadOnlyList<QueryRoomDto>? Rooms { get; set; }
}