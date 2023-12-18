using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class EstateRoomRepository: BaseRepository<EstateRoom>, IEstateRoomRepository
{
    public EstateRoomRepository(HomeHiveContext context) : base(context)
    {
    }

    public async Task<Result<EstateRoom>> FindBy(Guid estateId, Guid roomId)
    {
        var result = await Context.Set<EstateRoom>()
            .FirstOrDefaultAsync(x => x.EstateId == estateId && x.RoomId == roomId);
        
        return result == null ? Result<EstateRoom>.Failure($"EstateRoom not found") : Result<EstateRoom>.Success(result);
    }
}