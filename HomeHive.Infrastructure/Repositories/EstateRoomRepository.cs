using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Infrastructure.Repositories;

public class EstateRoomRepository: BaseRepository<EstateRoom>, IEstateRoomRepository
{
    public EstateRoomRepository(HomeHiveContext context) : base(context)
    {
    }
}