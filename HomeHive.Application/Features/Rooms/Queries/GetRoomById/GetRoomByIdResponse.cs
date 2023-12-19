using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdResponse: BaseResponse
{
    public QueryRoomDto? Room { get; set; }
}