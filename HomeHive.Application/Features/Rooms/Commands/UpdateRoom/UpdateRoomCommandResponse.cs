using HomeHive.Application.Responses;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandResponse: BaseResponse
{
    public CreateRoomDto Room { get; set; } = default!;
}