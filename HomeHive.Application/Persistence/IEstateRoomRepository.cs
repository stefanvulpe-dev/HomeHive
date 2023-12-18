using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Persistence;

public interface IEstateRoomRepository : IAsyncRepository<EstateRoom>
{
    public Task<Result<EstateRoom>> FindBy(Guid estateId, Guid roomId);
}
