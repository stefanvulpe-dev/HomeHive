using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Rooms.Commands.DeleteRoomById;

public record DeleteRoomByIdCommand(Guid RoomId) : ICommand<DeleteRoomByIdCommandResponse>;
