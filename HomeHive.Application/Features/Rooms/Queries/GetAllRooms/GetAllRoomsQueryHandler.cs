using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository): IQueryHandler<GetAllRoomsQuery, GetAllRoomsResponse>
{
    public async Task<GetAllRoomsResponse> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllAsync();
        if (!rooms.IsSuccess)
            return new GetAllRoomsResponse
            {
                IsSuccess = false,
                Message = "Rooms not found."
            };

        IReadOnlyList<QueryRoomDto>? mappedRooms = rooms.Value.Select(room =>
            new QueryRoomDto
            {
                Id = room.Id,
                RoomType = room.RoomType.ToString()
            }).ToList();
        
        return new GetAllRoomsResponse { Rooms = mappedRooms };
    }
}