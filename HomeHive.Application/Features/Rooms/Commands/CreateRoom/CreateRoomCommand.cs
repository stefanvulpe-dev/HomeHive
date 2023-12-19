using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Rooms;

namespace HomeHive.Application.Features.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(string RoomType): ICommand<CreateRoomCommandResponse>;
