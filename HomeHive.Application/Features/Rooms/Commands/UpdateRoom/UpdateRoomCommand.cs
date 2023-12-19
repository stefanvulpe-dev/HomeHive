using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Application.Features.Rooms.Commands.UpdateRoom;

public record UpdateRoomCommand(Guid Id, string RoomType): ICommand<UpdateRoomCommandResponse>;
