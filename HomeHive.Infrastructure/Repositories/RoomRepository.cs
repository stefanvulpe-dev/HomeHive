using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class RoomRepository : BaseRepository<Room>
{
    public RoomRepository(HomeHiveContext context) : base(context)
    {
        
    }
}