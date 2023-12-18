using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Rooms.Queries.GetRoomById;

public record GetRoomByIdQuery(Guid Id): IQuery<GetRoomByIdResponse>;