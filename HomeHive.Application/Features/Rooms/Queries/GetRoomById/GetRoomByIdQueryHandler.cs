using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler(IRoomRepository roomRepository): IQueryHandler<GetRoomByIdQuery, GetRoomByIdResponse>
{
    public async Task<GetRoomByIdResponse> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.FindByIdAsync(request.Id);
        if (!room.IsSuccess)
            return new GetRoomByIdResponse
            {
                IsSuccess = false,
                Message = "Room not found."
            };

        return new GetRoomByIdResponse
        {
            IsSuccess = true,
            Room = new QueryRoomDto
            {
                Id = room.Value.Id,
                RoomType = room.Value.RoomType.ToString()
            }
        };
    }
}