using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandResponse: BaseResponse
{
    public CreateRoomDto Room { get; set; } = default!;
}